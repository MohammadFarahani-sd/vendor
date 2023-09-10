using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OFood.Shop.Query.Models.Customers;

namespace OFood.Shop.Query.EntityConfigurations.Customers;

public class CustomerEntityTypeConfiguration : EntityTypeConfiguration<Customer>
{
    public override void ConfigureDerived(EntityTypeBuilder<Customer> configuration)
    {
        configuration.ToTable("Customers");

        configuration.HasKey(o => o.Id);

        var customerAddressNavigation = configuration.Metadata.FindNavigation(nameof(Customer.CustomerAddresses));

        customerAddressNavigation!.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}