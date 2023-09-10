using Autofac;
using Common.BuildingBlocks.EventBus;
using Common.BuildingBlocks.EventBus.Abstractions;
using Common.BuildingBlocks.EventBusRabbitMQ;
using Common.BuildingBlocks.IntegrationEventLogEF.Services;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Data.Common;

namespace OFood.Shop.Api.Configurations.EventBus;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RabbitRegisterServiceCollection(this IServiceCollection services,
        IConfigurationSection configurationSection)
    {
        services.Configure<EventBusSetting>(configurationSection);

        services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
        {
            var persistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
            var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
            var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
            var eventBusSubscriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();
            var subscriptions = sp.GetRequiredService<IOptions<EventBusSubscriptionClients>>().Value;
            var settings = sp.GetRequiredService<IOptions<EventBusSetting>>().Value;

            var retryCount = 5;
            if (!string.IsNullOrEmpty(settings.RetryCount)) retryCount = int.Parse(settings.RetryCount);

            return new EventBusRabbitMQ(persistentConnection, logger, iLifetimeScope,
                eventBusSubscriptionsManager, settings.BrokerPrefixName, subscriptions.SubscriptionClientName,
                retryCount);
        });

        services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();


        return services;
    }

    public static IServiceCollection AddIntegrationServices(this IServiceCollection services)
    {
        services.AddTransient<Func<DbConnection, IIntegrationEventLogService>>(
                sp => (DbConnection c) => new IntegrationEventLogService(c));

        services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<EventBusSetting>>().Value;
            var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();

            var factory = new ConnectionFactory
            {
                HostName = settings.HostName,
                DispatchConsumersAsync = true,
                Port = settings.Port
            };

            if (!string.IsNullOrEmpty(settings.Username)) factory.UserName = settings.Username;

            if (!string.IsNullOrEmpty(settings.Password)) factory.Password = settings.Password;

            if (!string.IsNullOrEmpty(settings.VirtualHost)) factory.VirtualHost = settings.VirtualHost;

            var retryCount = 5;
            if (!string.IsNullOrEmpty(settings.RetryCount)) retryCount = int.Parse(settings.RetryCount);

            return new DefaultRabbitMQPersistentConnection(factory, logger, retryCount);
        });

        return services;
    }

    public static IServiceCollection AddCustomHealthCheck(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHealthChecks()
            //.AddRabbitMQ(rabbitConnectionString: $"amqp://{configuration.GetValue<string>("EventBusSetting:username")}:{configuration.GetValue<string>("EventBusSetting:password")}@{configuration.GetValue<string>("EventBusSetting:hostName")}:{configuration.GetValue<string>("EventBusSetting:port")}/{configuration.GetValue<string>("EventBusSetting:VirtualHost")}")
            .AddRedis($"{configuration.GetValue<string>("Redis:Connection")!.Split(":").FirstOrDefault()}:{configuration.GetValue<string>("Redis:Connection")!.Split(":").LastOrDefault()},password={configuration.GetValue<string>("Redis:Password")},ssl={configuration.GetValue<string>("Redis:usessl")!.ToLower()}");

        return services;
    }
}
