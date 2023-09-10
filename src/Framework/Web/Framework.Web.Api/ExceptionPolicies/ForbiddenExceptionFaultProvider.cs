using Framework.Core.Security.Authorization;

namespace Framework.Web.Api.ExceptionPolicies;

public class ForbiddenExceptionFaultProvider : HttpFaultProvider
{
    public override bool IsEligible(Exception ex)
    {
        return ex is ForbiddenException;
    }

    protected override int GetStatusCode(Exception ex)
    {
        return 403;
    }

    protected override string GetMessage(Exception ex)
    {
        return ex.Message;
    }
}