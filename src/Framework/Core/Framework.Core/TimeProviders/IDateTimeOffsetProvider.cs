namespace Framework.Core.TimeProviders;

public interface IDateTimeOffsetProvider
{
    DateTimeOffset UtcNow { get; }
}