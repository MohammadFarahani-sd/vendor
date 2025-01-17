﻿using System;
using System.Linq;
using Caching.Abstractions;
using Caching.InMemory;
using Caching.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CacheHelper
{
    public static class ServiceCollectionExtension
    {
        public static void AddCacheServices(this IServiceCollection services,
            IConfigurationSection configurationSection)
        {
            services.Configure<CacheConfiguration>(configurationSection);

            var cacheProviders = configurationSection.GetSection("Providers").GetChildren().Select(x => x.Value)
                .ToArray();
            if (cacheProviders.Length == 0)
                services.AddScoped<NullCacheProvider>();
            else
                foreach (var cacheProvider in cacheProviders)
                    switch (cacheProvider.ToLower())
                    {
                        case "inmemory":
                            services.AddScoped<ICacheProvider, ImmutableWithMapperInMemoryCacheProvider>();
                            services.AddMemoryCache();
                            break;

                        case "redis":
                            services.AddScoped<ICacheProvider, RedisCacheProvider>();
                            break;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }


            services.AddScoped<ICacheHelper, CacheHelper>();
        }
    }
}