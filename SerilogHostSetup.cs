/*----------------------------------------------------------------
 * 命名空间：Net7.Core
 * 创建者：WangBenChi
 * 创建时间：2023/04/12 14:35:23
 *----------------------------------------------------------------*/

using System.Net;
using Masuit.Tools;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Net7.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Elasticsearch;
using Serilog.Sinks.Elasticsearch;

namespace Net7.Core
{
    public static class SerilogHostSetup
    {

        /// <summary>
        /// 添加Serilog
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configuration"></param>
        public static void AddSerilogHost(this IHostBuilder builder, IServiceCollection serviceProvider, ConfigurationManager configuration)
        {
            var options = configuration.GetSection(SerilogOptions.Position).Get<SerilogOptions>();
            Log.Logger = new LoggerConfiguration()
                    .ConfigureFile(options.File)
                    .ConfigureConsole(options.Console)
                    .ConfigureElasticsearch(options.Elasticsearch)
                    .CreateLogger();
            builder.UseSerilog();
        }

        /// <summary>
        /// 添加Serilog中间件
        /// </summary>
        /// <param name="app"></param>
        public static void UseSerilogSetup(this IApplicationBuilder app)
        {
            app.UseSerilogRequestLogging();
        }

        static LoggerConfiguration ConfigureFile(this LoggerConfiguration logger, SerilogFileOptions options)
        {
            if (options == null || options.Minlevels == null || !options.Minlevels.Any())
                return logger;

            //var path = AppDomain.CurrentDomain.BaseDirectory + "/Serilog/" + DateTime.Now.ToString(("yyyy-MM-dd")) + ".txt";
            var path = "Serilog/" + ".txt";
            foreach (var item in options.Minlevels)
            {
                logger.WriteTo.File(
                        path: path,
                        outputTemplate: options.Template,
                        rollingInterval: RollingInterval.Day,
                        shared: true,
                        fileSizeLimitBytes: 10 * 1024 * 1024,
                        rollOnFileSizeLimit: true,
                        restrictedToMinimumLevel: Enum.Parse<LogEventLevel>(item)
                        );
            }
            return logger;
        }
        static LoggerConfiguration ConfigureConsole(this LoggerConfiguration logger, SerilogConsoleOptions options)
        {
            if (options == null) return logger;
            logger.WriteTo.Console(
                        restrictedToMinimumLevel: Enum.Parse<LogEventLevel>(options.Minlevel),
                        outputTemplate: options.Template
                        );
            return logger;
        }

        static LoggerConfiguration ConfigureElasticsearch(this LoggerConfiguration logger, SerilogElasticsearchOptions options)
        {
            if (options == null||options.Nodes.FirstOrDefault().IsNullOrEmpty()) return logger;
            List<Uri> uris = options.Nodes.Select(x => new Uri(x)).ToList();
            logger.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(uris)
            {
                IndexFormat = options.Indexformat,
                NumberOfShards = options.NumberOfShards, // 主分片数
                NumberOfReplicas = options.NumberOfReplicas,// 复分片数
                CustomFormatter = new ExceptionAsObjectJsonFormatter(renderMessage: true),
                EmitEventFailure = EmitEventFailureHandling.RaiseCallback, // 消息发送失败时，执行回调函数 FailureCallback
                FailureCallback = FailureCallback,
                AutoRegisterTemplate = true,
                AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv8,
                ModifyConnectionSettings = conn =>
                {
                    if (options.UserName.IsNullOrEmpty()||options.Password.IsNullOrEmpty())
                    {
                        conn.BasicAuthentication(options.UserName, options.Password);
                    }
                    return conn;
                }
            });
            return logger;
        }
        static void FailureCallback(LogEvent e)
        {
            Console.WriteLine("日志记录失败，信息：" + e.MessageTemplate);
        }
        //static void ConfigureEmail(this LoggerConfiguration serilogConfig,SerilogEmailOptions options)
        //{
        //    if (options == null) return;
        //    serilogConfig.WriteTo.Email(
        //                connectionInfo: new EmailConnectionInfo
        //                {
        //                    EnableSsl = true,
        //                    Port = options.Port,
        //                    FromEmail = options.FromEmail,
        //                    ToEmail = options.ToEmail,
        //                    MailServer = options.MailServer,
        //                    EmailSubject = options.EmailSubject,
        //                    NetworkCredentials = new NetworkCredential(options.UserName, options.Password)
        //                },
        //                restrictedToMinimumLevel: LogEventLevel.Error);
        //}
    }
}