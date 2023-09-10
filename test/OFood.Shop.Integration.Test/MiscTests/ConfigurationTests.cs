using FluentAssertions;
using OFood.Shop.Integration.Test.BaseTests;
using Xunit;

namespace OFood.Shop.Integration.Test.MiscTests;

public class ConfigurationTests : BaseIntegrationTest
{
    [Fact]
    public void Test_sentry_dsn_is_null_or_empty()
    {
        var testSentryDsn = Configuration.GetSection("Sentry__Dsn").Value;
        testSentryDsn.Should().BeNullOrEmpty();
    }
}
