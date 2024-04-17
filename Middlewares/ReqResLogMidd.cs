using Masuit.Tools;

namespace Gateway.Middlewares
{
    /// <summary>
    /// 请求与响应数据中间件
    /// </summary>
    public class ReqResLogMidd
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ReqResLogMidd> _logger;

        /// <summary>
        /// 初始化 ReqResLogMidd 类的新实例
        /// </summary>
        /// <param name="next">请求委托</param>
        public ReqResLogMidd(RequestDelegate next, ILogger<ReqResLogMidd> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// 执行中间件操作
        /// </summary>
        /// <param name="context">HTTP 上下文</param>
        /// <returns>异步任务</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            //上传体中包含文件时跳过
            //判断请求类型
            if (context.Request.ContentType != null && (context.Request.ContentType.Contains("multipart/form-data") || context.Request.ContentType.Contains("x-www-form-urlencoded")))
            {
                if (!context.Request.Form.Files.IsNullOrEmpty())
                {
                    await _next(context);
                    return;
                }
            }

            //路径包含swagger时跳过
            if (context.Request.Path.Value.Contains("swagger"))
            {
                await _next(context);
                return;
            }
            else
            {
                await Console.Out.WriteLineAsync($"--------------------------------------------------------------------");
                //时间
                await Console.Out.WriteLineAsync($" 时间: {DateTime.Now:G}");
                // 启用请求体缓冲
                context.Request.EnableBuffering();
                // 获取原始请求体
                Stream originalBody = context.Response.Body;
                try
                {
                    // 设置请求数据
                    await RequestDataLog(context);

                    using (var ms = new MemoryStream())
                    {
                        // 设置返回内容
                        context.Response.Body = ms;
                        // 调用下一个中间件
                        await _next(context);
                        await ResponseDataLog(context.Response);
                        // 复制响应数据
                        ms.Position = 0;
                        await ms.CopyToAsync(originalBody);
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError($"异常信息:{e.Message}");
                }
                finally
                {
                    //context.Request.Body = originalBody;
                }
                await Console.Out.WriteLineAsync("--------------------------------------------------------------------");
            }
        }

        /// <summary>
        /// 记录请求数据
        /// </summary>
        /// <param name="context">HTTP 上下文</param>
        /// <returns>异步任务</returns>
        private async Task RequestDataLog(HttpContext context)
        {
            var request = context.Request;
            var sr = new StreamReader(request.Body);

            var content = @$" 请求路径:{request.Path + request.QueryString}
    请求类型：{request.ContentType}
    请求方法：{request.Method}
    Body数据:{await sr.ReadToEndAsync()}
                                 ";

            await Console.Out.WriteLineAsync($"{content}");
            request.Body.Position = 0;
        }

        /// <summary>
        /// 记录响应数据
        /// </summary>
        /// <param name="response">HTTP 响应</param>
        /// <param name="ms">内存流</param>
        private async Task ResponseDataLog(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var ResponseBody = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            //状态码
            await Console.Out.WriteLineAsync($" 响应状态码: {response.StatusCode}");
            //响应数据
            await Console.Out.WriteLineAsync($" 响应数据: \n {ResponseBody}");
        }
    }
}