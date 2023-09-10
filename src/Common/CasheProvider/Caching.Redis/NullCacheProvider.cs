using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Caching.Abstractions;

namespace Caching.Redis
{
    public class NullCacheProvider : ICacheProvider
    {
        public Task StoreAsync<T>(string key, T value, TimeSpan? expiration = null, TimeSpan? ttl = null)
        {
            return Task.CompletedTask;
        }

        public Task SetExpirationAsync<T>(string key, TimeSpan expiration, TimeSpan? ttl = null)
        {
            return Task.CompletedTask;
        }

        public Task<CacheItem<T>> FetchAsync<T>(string key)
        {
            return Task.FromResult((CacheItem<T>)null);
        }

        public Task<CacheItem<T>> FetchAsync<T>(string key,
            TimeSpan? reconstructWindow,
            TimeSpan? expiration = null,
            TimeSpan? ttl = null)
        {
            return Task.FromResult((CacheItem<T>)null);
        }

        public Task RemoveAsync(string key, TimeSpan? expiration = null)
        {
            return Task.CompletedTask;
        }

        public Task<List<PersonLocation>> GetLocationsFilterdAsync(string key, double latitude, double longitude, double radius)
        {
            return Task.FromResult((List<PersonLocation>)null);
        }

        public Task AddLocationAsync(string key, double latitude, double longitude, string prefixStatus, TimeSpan? expiration = null, TimeSpan? ttl = null)
        {
            return Task.CompletedTask;
        }
    }
}