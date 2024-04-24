using Masuit.Tools;

namespace Gateway.Middlewares
{
    /// <summary>
    /// 请求过滤
    /// </summary>
    /// <param name="next"></param>
    /// <param name="logger"></param>
    public class ReqIgnoreMidd
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ReqResLogMidd> _logger;

        /// <summary>
        /// 请求过滤
        /// </summary>
        /// <param name="next"></param>
        /// <param name="logger"></param>
        public ReqIgnoreMidd(RequestDelegate next, ILogger<ReqResLogMidd> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// 调用过滤
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            //过滤请求路径是/的
            if (context.Request.Path.Value == "/")
            {
                await _next(context);
                return;
            }
            //根据请求路径过滤,如果是swagger则不走网关直接返回
            if (context.Request != null || context.Request.Path.Value.Contains("swagger"))
            {
                await _next(context);
                return;
            }
            //过滤请求体中包含文件的请求
            if (context.Request.ContentType != null && (context.Request.ContentType.Contains("multipart/form-data") || context.Request.ContentType.Contains("x-www-form-urlencoded")))
            {
                if (!context.Request.Form.Files.IsNullOrEmpty())
                {
                    await _next(context);
                    return;
                }
            }
        }
    }
}