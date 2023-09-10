using Microsoft.EntityFrameworkCore;
using OFood.Shop.Query.EntityConfigurations.Customers;
using OFood.Shop.Query.Models.AreaCities;
using OFood.Shop.Query.Models.Customers;


namespace OFood.Shop.Query;

public class QueryDbContext : DbContext
{
    public QueryDbContext(DbContextOptions<QueryDbContext> options) : base(options)
    {
    }

    protected QueryDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Customer> Customers{ get; set; }
    public DbSet<CustomerAddress> CustomerAddresses { get; set; }
    public DbSet<AreaCity> AreaCities { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
    
        builder.ApplyConfiguration(new CustomerAddressEntityTypeConfiguration());
        builder.ApplyConfiguration(new CustomerEntityTypeConfiguration());

        builder.Entity<CustomerAddress>()
            .HasOne(a => a.City)
            .WithOne(a => a.CustomerAddress)
            .HasForeignKey<CustomerAddress>(c => c.CityId);
    }
}