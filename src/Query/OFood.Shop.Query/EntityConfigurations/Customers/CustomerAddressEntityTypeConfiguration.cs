using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetTopologySuite.Geometries;
using OFood.Shop.Query.Models.Customers;

namespace OFood.Shop.Query.EntityConfigurations.Customers;

public class CustomerAddressEntityTypeConfiguration : EntityTypeConfiguration<CustomerAddress>
{
    public override void ConfigureDerived(EntityTypeBuilder<CustomerAddress> configuration)
    {
        configuration.ToTable("CustomerAddresses");

        configuration.HasKey(o => o.Id);

        configuration.Property(c => c.CityId).IsRequired();
        configuration.Property(c => c.AreaId).IsRequired();
    }
}
