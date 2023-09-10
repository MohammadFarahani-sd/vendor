using Framework.Core.Logging;
using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Retry;
using OFood.Shop.Query;
using Common.BuildingBlocks.IntegrationEventLogEF;
using OFood.Shop.Infrastructure.Persistence;
using UserManagement.Security;

namespace OFood.Shop.Api.Seed
{
    public class ShopDbContextSeed
    {
        public async Task SeedMigrationAsync(InfrastructureDbContext context, IWebHostEnvironment env, ILogWriter<ShopDbContextSeed> logger)
        {
            var policy = CreatePolicy(logger, nameof(ShopDbContextSeed));
            if (env.EnvironmentName == "Test")
            {
                await policy.ExecuteAsync(async () =>
                {
                    await context.Database.MigrateAsync();
                });
            }
            else
            {
                await policy.ExecuteAsync(async () =>
                {
                    await context.Database.MigrateAsync();
                });
            }
        }

        public async Task SeedMigrationAsync(IntegrationEventLogContext context, ILogWriter<ShopDbContextSeed> logger)
        {
            var policy = CreatePolicy(logger, nameof(ShopDbContextSeed));

            await policy.ExecuteAsync(async () =>
            {
                await context.Database.MigrateAsync();
            });
        }

        public async Task SeedMigrationAsync(ApplicationDbContext context, IWebHostEnvironment env, ILogWriter<ShopDbContextSeed> logger)
        {
            var policy = CreatePolicy(logger, nameof(ShopDbContextSeed));
            if (env.EnvironmentName == "Test")
            {
                await policy.ExecuteAsync(async () =>
                {
                    await context.Database.MigrateAsync();
                });
            }
            else
            {
                await policy.ExecuteAsync(async () =>
                {
                    await context.Database.MigrateAsync();
                });
            }
        }

        private AsyncRetryPolicy CreatePolicy(ILogWriter<ShopDbContextSeed> logger, string prefix, int retries = 2)
        {
            return Policy.Handle<Exception>().WaitAndRetryAsync(
                retries,
                sleepDurationProvider => TimeSpan.FromSeconds(15),

                (exception, retry) =>
                {
                    Console.WriteLine(exception.InnerException);
                    logger.LogError(exception.Message);
                }
            );
        }
    }
}