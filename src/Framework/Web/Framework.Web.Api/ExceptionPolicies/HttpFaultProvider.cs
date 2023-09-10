using System.Text.Json;
using System.Text.Json.Serialization;
using Framework.Core.ExceptionHandling;
using Framework.Core.Logging;
using Framework.Web.Api.Models;
using Microsoft.AspNetCore.Http;

namespace Framework.Web.Api.ExceptionPolicies;

public abstract class HttpFaultProvider : IExceptionPolicy
{
    public virtual int Order => 0;

    public abstract bool IsEligible(Exception ex);

    public async void Apply(object context, Exception ex, ILogWriter<ExceptionHandler> logWriter)
    {
        await PrepareResponse(context, ex);
        LogException(logWriter, ex);
    }

    protected abstract int GetStatusCode(Exception ex);

    protected abstract string GetMessage(Exception ex);

    protected virtual int GetMetaCode(Exception ex)
    {
        return GetStatusCode(ex);
    }

    protected virtual void LogException(ILogWriter<ExceptionHandler> logWriter, Exception ex)
    {
        logWriter.LogInformation(ex.Message);
    }

    private async Task PrepareResponse(object context, Exception ex)
    {
        if (context is HttpContext httpContext)
        {
            httpContext.Response.Clear();
            httpContext.Response.StatusCode = GetStatusCode(ex);
            httpContext.Response.ContentType = "application/json";

            var responseModel = new Response(new ResponseMeta()
            {
                Code = GetMetaCode(ex),
                Message = GetMessage(ex),
                MessageType = MessageType.Error
            });

            var response = JsonSerializer.Serialize(responseModel, new JsonSerializerOptions
            {
                Converters = {new JsonStringEnumConverter()},
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await httpContext.Response.WriteAsync(response);
        }
    }
}