using Framework.Core.Domain.Exceptions;

namespace Framework.Web.Api.ExceptionPolicies;

public class DomainExceptionFaultProvider : HttpFaultProvider
{
    public override bool IsEligible(Exception ex)
    {
        return ex is DomainException;
    }

    protected override int GetStatusCode(Exception ex)
    {
        return 422;
    }

    protected override string GetMessage(Exception ex)
    {
        return ex.Message;
    }

    protected override int GetMetaCode(Exception ex)
    {
        return ((DomainException)ex).ErrorCode;
    }
}