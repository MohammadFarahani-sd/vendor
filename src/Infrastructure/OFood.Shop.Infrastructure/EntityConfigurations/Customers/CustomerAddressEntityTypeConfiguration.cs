using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetTopologySuite.Geometries;
using OFood.Shop.Domain.AggregatesModel.CustomerAggregate;
namespace OFood.Shop.Infrastructure.EntityConfigurations.Customers;

public class CustomerAddressEntityTypeConfiguration : EntityTypeConfiguration<CustomerAddress>
{
    public override void ConfigureDerived(EntityTypeBuilder<CustomerAddress> configuration)
    {
        configuration.ToTable("CustomerAddresses");

        configuration.HasKey(o => o.Id);

        configuration.Property<Guid>(c => c.CustomerId)
            .IsRequired();

        configuration.Property<int>(c => c.AreaId)
            .IsRequired();

        configuration.Property<int>(c => c.CityId)
            .IsRequired();

        configuration
            .Property<string?>(c => c.ExtraInfo)
            .HasMaxLength(1024);

        configuration
            .Property(c => c.Latitude);

        configuration
            .Property(c => c.Longitude);

    }
}
