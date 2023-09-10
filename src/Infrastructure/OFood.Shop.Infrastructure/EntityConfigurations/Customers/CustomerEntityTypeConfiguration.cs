using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OFood.Shop.Domain.AggregatesModel.CustomerAggregate;

namespace OFood.Shop.Infrastructure.EntityConfigurations.Customers;
public class CustomerEntityTypeConfiguration : EntityTypeConfiguration<Customer>
{
    public override void ConfigureDerived(EntityTypeBuilder<Customer> configuration)
    {
        configuration.ToTable("Customers");

        configuration.Property(o => o.Id).HasColumnName("Id");
        configuration.HasKey(o => o.Id);

        configuration
              .Property<string>(c => c.PhoneNumber)
              .HasMaxLength(256)
              .UsePropertyAccessMode(PropertyAccessMode.Field)
              .IsRequired();

        var customerAddressNavigation = configuration.Metadata.FindNavigation(nameof(Customer.CustomerAddresses));

        customerAddressNavigation!.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
