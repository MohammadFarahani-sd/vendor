namespace Framework.Web.Api.ExceptionPolicies;

public class ArgumentNullExceptionFaultProvider : HttpFaultProvider
{
    public override bool IsEligible(Exception ex)
    {
        return ex is ArgumentNullException;
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