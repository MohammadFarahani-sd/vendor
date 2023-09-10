using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Caching.Abstractions;
using System.Collections.Generic;

namespace Caching.InMemory
{
    public class InMemoryCacheProvider : ICacheProvider
    {

        private readonly IMemoryCache _memoryCache;

        public InMemoryCacheProvider(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public Task StoreAsync<T>(string key, T value, TimeSpan? expiration = null, TimeSpan? ttl = null)
        {
            var cacheItem = new CacheItem<T>(value, expiration);

            if (ttl.HasValue)
                _memoryCache.Set(key, cacheItem, DateTimeOffset.Now.Add(ttl.Value));
            else
                _memoryCache.Set(key, cacheItem);

            return Task.CompletedTask;
        }

        public Task SetExpirationAsync<T>(string key, TimeSpan expiration, TimeSpan? ttl = null)
        {
            if (_memoryCache.TryGetValue(key, out ICacheItem value))
            {
                value.ExpirationTicks = DateTime.Now.Add(expiration).Ticks;

                if (ttl.HasValue)
                    _memoryCache.Set(key, value, DateTimeOffset.Now.Add(ttl.Value));
                else
                    _memoryCache.Set(key, value);
            }

            return Task.CompletedTask;

        }

        public Task<CacheItem<T>> FetchAsync<T>(string key)
        {
            if (_memoryCache.TryGetValue(key, out CacheItem<T> value))
                return Task.FromResult(value);

            return Task.FromResult(default(CacheItem<T>));
        }

        public Task<CacheItem<T>> FetchAsync<T>(string key, TimeSpan? reconstructWindow, TimeSpan? expiration = null,
            TimeSpan? ttl = null)
        {

            if (_memoryCache.TryGetValue(key, out CacheItem<T> value))
            {
                if (!value.Expiration.HasValue || !value.IsExpired || !reconstructWindow.HasValue)
                    return Task.FromResult(value);

                var actualExpiration = value.Expiration.Value;

                value.ExpirationTicks = actualExpiration.Add(reconstructWindow.Value).Ticks;

                if (ttl.HasValue)
                    _memoryCache.Set(key, value, DateTimeOffset.Now.Add(ttl.Value));
                else
                    _memoryCache.Set(key, value);

                return Task.FromResult(new CacheItem<T>()
                {
                    ExpirationTicks = actualExpiration.Ticks,
                    Data = value.Data
                });
            }

            return Task.FromResult(default(CacheItem<T>));
        }

        public Task RemoveAsync(string key, TimeSpan? removeAt = null)
        {
            if (!removeAt.HasValue)
                _memoryCache.Remove(key);

            else if (_memoryCache.TryGetValue(key, out ICacheItem value))
                _memoryCache.Set(key, value, DateTimeOffset.Now.Add(removeAt.Value));

            return Task.CompletedTask;
        }

        public Task AddLocationAsync(string key, double latitude, double longitude, string prefixStatus, TimeSpan? expiration = null, TimeSpan? ttl = null)
        {
            return Task.CompletedTask;
        }

        public Task<List<PersonLocation>> GetLocationsFilterdAsync(string key, double latitude, double longitude, double radius)
        {
            return Task.FromResult((List<PersonLocation>)null);
        }
    }
}
