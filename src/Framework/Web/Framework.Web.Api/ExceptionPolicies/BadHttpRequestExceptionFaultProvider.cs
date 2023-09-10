using Microsoft.AspNetCore.Http;

namespace Framework.Web.Api.ExceptionPolicies;

public class BadHttpRequestExceptionFaultProvider : HttpFaultProvider
{
    public override bool IsEligible(Exception ex)
    {
        return ex is BadHttpRequestException;
    }

    protected override int GetStatusCode(Exception ex)
    {
        return 400;
    }

    protected override string GetMessage(Exception ex)
    {
        return ex.Message;
    }
}