using Framework.Core.Logging;

namespace Framework.Core.ExceptionHandling;

public interface IExceptionPolicy
{
    int Order { get; }

    bool IsEligible(Exception ex);

    void Apply(object context, Exception ex, ILogWriter<ExceptionHandler> logWriter);
}