/*----------------------------------------------------------------
 * 命名空间：Net7.Core.Setups
 * 文件名：SerilogOptions
 * 创建者：WangBenChi
 * 电子邮箱：69945864@qq.com
 * 创建时间：2023/11/13 21:37:19
 *----------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net7.Configuration
{
    /// <summary>
    /// Serilog配置文件
    /// </summary>
    public class SerilogOptions
    {
        /// <summary>
        /// 定义一个常量字符串Position，表示日志记录器的位置
        /// </summary>
        public const string Position = "Serilog";

        /// 定义一个名为Override的属性，类型为List<SerilogOverrideOptions>，用于存储日志记录器的覆盖选项
        public List<SerilogOverrideOptions> Override { get; set; }

        /// <summary>
        /// 定义一个名为Console的属性，类型为SerilogConsoleOptions，用于配置控制台日志输出
        /// </summary>
        public SerilogConsoleOptions Console { get; set; }

        /// <summary>
        /// 定义一个名为File的属性，类型为SerilogFileOptions，用于配置文件日志输出
        /// </summary>
        public SerilogFileOptions File { get; set; }

        /// <summary>
        /// 定义一个名为Email的属性，类型为SerilogEmailOptions，用于配置邮件日志输出
        /// </summary>
        public SerilogEmailOptions Email { get; set; }

        /// <summary>
        /// 定义一个名为Elasticsearch的属性，类型为SerilogElasticsearchOptions，用于配置Elasticsearch日志输出
        /// </summary>
        public SerilogElasticsearchOptions Elasticsearch { get; set; }
    }

    /// <summary>
    /// 定义一个名为SerilogOverrideOptions的类，用于存储日志记录器的覆盖选项
    /// </summary>
    public class SerilogOverrideOptions
    {
        /// <summary>
        /// 定义一个名为Source的属性，类型为string，表示日志来源
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 定义一个名为Level的属性，类型为string，表示日志级别
        /// </summary>
        public string Level { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SerilogConsoleOptions
    {
        /// <summary>
        /// 记录最小等级
        /// </summary>
        public string Minlevel { get; set; }

        /// <summary>
        /// 模板
        /// </summary>
        public string Template { get; set; }
    }
    /// <summary>
    /// 定义一个名为SerilogFileOptions的类，用于存储日志文件的配置选项
    /// </summary>
    public class SerilogFileOptions
    {
        /// <summary>
        /// 模板字符串，用于格式化日志输出
        /// </summary>
        public string Template { get; set; }

        /// <summary>
        /// 最小日志级别数组，用于设置日志记录的最低级别
        /// </summary>
        public string[] Minlevels { get; set; }
    }

    /// <summary>
    /// 定义一个名为SerilogEmailOptions的类，用于存储邮件发送的配置选项
    /// </summary>
    public class SerilogEmailOptions
    {
        /// <summary>
        /// 邮件服务器端口号
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 发件人邮箱地址
        /// </summary>
        public string FromEmail { get; set; }

        /// <summary>
        /// 收件人邮箱地址
        /// </summary>
        public string ToEmail { get; set; }

        /// <summary>
        /// 邮件服务器地址
        /// </summary>
        public string MailServer { get; set; }

        /// <summary>
        /// 邮件主题
        /// </summary>
        public string EmailSubject { get; set; }

        /// <summary>
        /// 邮件用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 邮件密码
        /// </summary>
        public string Password { get; set; }
    }

    public class SerilogElasticsearchOptions
    {
        /// <summary>
        /// 节点地址
        /// </summary>
        public List<string> Nodes { get; set; }

        /// <summary>
        /// 索引模板
        /// </summary>
        public string Indexformat { get; set; } = "logstash-api-{0:yyyy.MM}";


        /// <summary>
        /// 主分片数
        /// </summary>
        public int NumberOfShards { get; set; } = 1;


        /// <summary>
        /// 副分片数
        /// </summary>
        public int NumberOfReplicas { get; set; } = 0;


        /// <summary>
        /// 账号
        /// </summary>
        public string UserName { get; set; } = "elastic";


        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
    }
}
