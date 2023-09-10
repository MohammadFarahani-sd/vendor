using Framework.Core.Security.Authorization;

namespace Framework.Web.Api.ExceptionPolicies;

public class UnauthorizedExceptionFaultProvider : HttpFaultProvider
{
    public override bool IsEligible(Exception ex)
    {
        return ex is UnauthorizedException;
    }

    protected override int GetStatusCode(Exception ex)
    {
        return 401;
    }

    protected override string GetMessage(Exception ex)
    {
        return ex.Message;
    }
}