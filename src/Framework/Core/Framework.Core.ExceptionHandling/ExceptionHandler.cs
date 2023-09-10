using Framework.Core.Logging;
namespace Framework.Core.ExceptionHandling;

public class ExceptionHandler : IExceptionHandler
{
    private readonly IEnumerable<IExceptionPolicy> _policies;
    private readonly ILogWriter<ExceptionHandler> _logWriter;

    public ExceptionHandler(IEnumerable<IExceptionPolicy> exceptionPolicies, ILogWriter<ExceptionHandler> logWriter)
    {
        _policies = exceptionPolicies;
        _logWriter = logWriter;
    }

    public void Handle(object context, Exception ex)
    {
        var eligiblePolicy = _policies.OrderBy(p => p.Order).First(p => p.IsEligible(ex));
        eligiblePolicy.Apply(context, ex, _logWriter);
    }
}