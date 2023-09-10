using Sentry;
using Sentry.Extensibility;

namespace Framework.Logging.Sentry;

public class SentryEventProcessor : ISentryEventProcessor
{
    public SentryEvent? Process(SentryEvent @event)
    {
        return @event;
    }
}