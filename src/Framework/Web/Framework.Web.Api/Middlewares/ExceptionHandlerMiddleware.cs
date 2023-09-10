using Framework.Core.ExceptionHandling;
using Framework.Core.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Framework.Web.Api.Middlewares;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate next;

    public ExceptionHandlerMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task Invoke(HttpContext httpContext,
        [FromServices] IExceptionHandler exceptionHandler,
        [FromServices] ILogWriter<ExceptionHandlerMiddleware> logWriter)
    {
        try
        {
            await next(httpContext);
        }
        catch (Exception ex)
        {
            HandleException(httpContext, ex, exceptionHandler, logWriter);
        }
    }

    private static void HandleException(HttpContext context, Exception ex, IExceptionHandler exceptionHandler, ILogWriter<ExceptionHandlerMiddleware> logWriter)
    {
        try
        {
            exceptionHandler.Handle(context, ex);
        }
        catch (Exception unSupportedException)
        {
            logWriter.LogCritical(unSupportedException, unSupportedException.Message);
        }
    }
}