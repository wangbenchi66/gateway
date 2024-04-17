using System.Net;
using Newtonsoft.Json;

namespace Gateway.Middlewares
{
    /// <summary>
    /// 中间件用于处理异常情况。
    /// </summary>
    public class ExceptionMidd
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMidd> _logger;

        /// <summary>
        /// 初始化 ExceptionMidd 类的新实例。
        /// </summary>
        /// <param name="next">请求委托。</param>
        /// <param name="logger">日志记录器。</param>
        public ExceptionMidd(RequestDelegate next, ILogger<ExceptionMidd> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// 执行中间件操作。
        /// </summary>
        /// <param name="context">HTTP 上下文。</param>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (OperationCanceledException ex)
            {
                await HandleExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        /// <summary>
        /// 处理异常情况。
        /// </summary>
        /// <param name="context">HTTP 上下文。</param>
        /// <param name="exception">异常对象。</param>
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            await context.Response.WriteAsync(JsonConvert.SerializeObject(new
            {
                code = context.Response.StatusCode,
                message = "Internal Server Error."
            }));
        }
    }
}