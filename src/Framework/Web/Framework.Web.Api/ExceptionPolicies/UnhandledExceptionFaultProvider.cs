using Framework.Core.ExceptionHandling;
using Framework.Core.Logging;
using Microsoft.Extensions.Hosting;

namespace Framework.Web.Api.ExceptionPolicies;

public class UnhandledExceptionFaultProvider : HttpFaultProvider
{
    public override int Order => 999;

    public override bool IsEligible(Exception ex)
    {
        return true;
    }

    protected override int GetStatusCode(Exception ex)
    {
        return 500;
    }

    protected override string GetMessage(Exception ex)
    {
        var aspNetCoreEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        return aspNetCoreEnvironment == Environments.Development ? ex.Message : "Server Error!!!";
    }

    protected override void LogException(ILogWriter<ExceptionHandler> logWriter, Exception ex)
    {
        logWriter.LogCritical(ex, ex.Message);
    }
}