using Framework.Core.TimeProviders;

namespace OFood.Shop.TestUtilities.TimeProviders;

public class TestDateTimeOffsetProvider : IDateTimeOffsetProvider, IConfigurableDateTimeOffsetProvider
{
    private bool _isManual;
    private DateTimeOffset _utcNow;

    public TestDateTimeOffsetProvider()
    {
        _isManual = false;
    }

    public void ResetUtcNow()
    {
        _utcNow = DateTimeOffset.UtcNow;
    }

    public void SetUtcNow(DateTimeOffset dateTimeOffset)
    {
        _isManual = true;
        _utcNow = dateTimeOffset;
    }

    public DateTimeOffset UtcNow => _isManual ? _utcNow : DateTimeOffset.UtcNow;
}