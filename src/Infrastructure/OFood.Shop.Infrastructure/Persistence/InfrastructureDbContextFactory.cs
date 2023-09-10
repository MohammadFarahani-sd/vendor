using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace OFood.Shop.Infrastructure.Persistence
{
    public class InfrastructureDbContextFactory : IDesignTimeDbContextFactory<InfrastructureDbContext>
    {
        public InfrastructureDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<InfrastructureDbContext>();
            var connectionString = args.Any() ? args[0] : "Data Source=(local);Initial Catalog=OFoodShop;Persist Security Info=True;User ID=sa;Password=09128986248M@M;MultipleActiveResultSets=True;Connect Timeout=30";

            builder.UseSqlServer(
                connectionString,
                db => db.MigrationsAssembly(typeof(InfrastructureDbContextFactory).Assembly.GetName().Name).UseNetTopologySuite());

            return new InfrastructureDbContext(builder.Options);
        }
    }
}