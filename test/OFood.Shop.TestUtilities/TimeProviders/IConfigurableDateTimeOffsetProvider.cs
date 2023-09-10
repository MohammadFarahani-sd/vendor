using Framework.Core.TimeProviders;

namespace OFood.Shop.TestUtilities.TimeProviders;

public interface IConfigurableDateTimeOffsetProvider : IDateTimeOffsetProvider
{
    void ResetUtcNow();
    void SetUtcNow(DateTimeOffset dateTimeOffset);
}