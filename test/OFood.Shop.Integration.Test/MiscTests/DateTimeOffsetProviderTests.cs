using System;
using FluentAssertions;
using Framework.Core.TimeProviders;
using Microsoft.Extensions.DependencyInjection;
using OFood.Shop.Integration.Test.BaseTests;
using OFood.Shop.TestUtilities.TimeProviders;
using Xunit;

namespace OFood.Shop.Integration.Test.MiscTests;

[Collection("none paralleled collection 1")]
[CollectionDefinition("none paralleled collection 1", DisableParallelization = true)]
public class DateTimeOffsetProviderTests : BaseIntegrationTest
{
    [Fact]
    public void Test_date_time_offset_provider_registration()
    {
        var dateTimeOffsetProvider = Application.Services.GetService<IDateTimeOffsetProvider>();
        dateTimeOffsetProvider.Should().NotBeNull();
    }

    [Fact]
    public void Test_configurable_date_time_offset_provider_registration()
    {
        var configurableDateTimeOffsetProvider = Application.Services.GetService<IConfigurableDateTimeOffsetProvider>();
        configurableDateTimeOffsetProvider.Should().NotBeNull();
    }

    [Fact]
    public void Test_date_time_offset_provider_is_equals_to_configurable_date_time_offset_provider()
    {
        var dateTimeOffsetProvider = Application.Services.GetService<IDateTimeOffsetProvider>();
        var configurableDateTimeOffsetProvider = Application.Services.GetService<IConfigurableDateTimeOffsetProvider>();
        dateTimeOffsetProvider.Should().BeEquivalentTo(configurableDateTimeOffsetProvider);
    }

    [Fact]
    public void Test_date_time_offset_provider_date_is_equals_to_configurable_date_time_offset_provider()
    {
        var dateTimeOffsetProvider = Application.Services.GetService<IDateTimeOffsetProvider>()!;
        var configurableDateTimeOffsetProvider = Application.Services.GetService<IConfigurableDateTimeOffsetProvider>()!;

        configurableDateTimeOffsetProvider.SetUtcNow(DateTimeOffset.UtcNow.AddHours(1));

        dateTimeOffsetProvider.UtcNow.Should().Be(configurableDateTimeOffsetProvider.UtcNow);
    }

}