using System.Globalization;
using Framework.Core.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Framework.Web.Api.Middlewares;

public class LocalizationMiddleware
{
    private readonly RequestDelegate next;

    public LocalizationMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task Invoke(HttpContext httpContext, [FromServices] ILogWriter<LocalizationMiddleware> logWriter)
    {
        var acceptLanguage = httpContext.Request.Headers["Accept-Language"].FirstOrDefault()?.Split(",").FirstOrDefault();

        if (acceptLanguage != null)
        {
            try
            {
                CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo(acceptLanguage);
            }
            catch (Exception ex)
            {
                logWriter.LogInformation(ex.Message);
            }
        }

        await next(httpContext);
    }
}