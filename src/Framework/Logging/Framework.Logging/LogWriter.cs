using Framework.Core.Logging;
using Microsoft.Extensions.Logging;

namespace Framework.Logging;

public class LogWriter<T> : ILogWriter<T>
{
    private readonly ILogger _logger;

    public LogWriter(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<T>();
    }

    public void LogTrace(string? message, params object?[] args)
    {
        _logger.LogTrace(message, args);
    }

    public void LogDebug(string? message, params object?[] args)
    {
        _logger.LogDebug(message, args);
    }

    public void LogInformation(string? message, params object?[] args)
    {
        _logger.LogInformation(message, args);
    }

    public void LogWarning(string? message, params object?[] args)
    {
        _logger.LogWarning(message, args);
    }

    public void LogError(string? message, params object?[] args)
    {
        _logger.LogError(message, args);
    }

    public void LogCritical(string? message, params object?[] args)
    {
        _logger.LogCritical(message, args);
    }

    public void LogCritical(Exception? exception, string? message, params object?[] args)
    {
        _logger.LogCritical(exception, message, args);
    }
}