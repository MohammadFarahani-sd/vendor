using Sentry;
using Sentry.Extensibility;

namespace Framework.Logging.Sentry;

public class SentryEventExceptionProcessor : ISentryEventExceptionProcessor
{
    public void Process(Exception exception, SentryEvent sentryEvent)
    {
    }
}