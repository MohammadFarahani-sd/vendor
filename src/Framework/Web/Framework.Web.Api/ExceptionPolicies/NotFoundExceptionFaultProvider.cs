using Framework.Core.Exceptions;

namespace Framework.Web.Api.ExceptionPolicies;

public class NotFoundExceptionFaultProvider : HttpFaultProvider
{
    public override bool IsEligible(Exception ex)
    {
        return ex is NotFoundException;
    }

    protected override int GetStatusCode(Exception ex)
    {
        return 404;
    }

    protected override string GetMessage(Exception ex)
    {
        return ex.Message;
    }
}