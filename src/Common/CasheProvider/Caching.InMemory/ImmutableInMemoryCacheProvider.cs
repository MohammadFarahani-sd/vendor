using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Caching.Abstractions;
using Serializer;
using System.Collections.Generic;

namespace Caching.InMemory
{
    /// <summary>
    /// Using serializer to implement immutable in memory cache,All the types which will be used in caching,must be compatible and able to serialize with registered 
    /// </summary>
    public class ImmutableInMemoryCacheProvider : ICacheProvider
    {

        private readonly IMemoryCache _memoryCache;
        private readonly ISerializer _serializer;

        public ImmutableInMemoryCacheProvider(IMemoryCache memoryCache, ISerializer serializer)
        {
            _memoryCache = memoryCache;
            _serializer = serializer;
        }

        public Task StoreAsync<T>(string key, T value, TimeSpan? expiration = null, TimeSpan? ttl = null)
        {
            var cacheItem = new CacheItem<T>(value, expiration);

            var valueToStore = _serializer.Serialize(cacheItem);

            if (ttl.HasValue)
                _memoryCache.Set(key, valueToStore, DateTimeOffset.Now.Add(ttl.Value));
            else
                _memoryCache.Set(key, valueToStore);

            return Task.CompletedTask;
        }

        public Task SetExpirationAsync<T>(string key, TimeSpan expiration, TimeSpan? ttl = null)
        {
            if (_memoryCache.TryGetValue(key, out byte[] storedObject))
            {
                var value = _serializer.Deserialize<CacheItem<T>>(storedObject);

                value.ExpirationTicks = DateTime.Now.Add(expiration).Ticks;

                return StoreAsync(key, value.Data, expiration, ttl);
            }

            return Task.CompletedTask;

        }

        public Task<CacheItem<T>> FetchAsync<T>(string key)
        {
            if (_memoryCache.TryGetValue(key, out byte[] storedValue))
            {
                var value = _serializer.Deserialize<CacheItem<T>>(storedValue);

                return Task.FromResult(value);
            }

            return Task.FromResult(default(CacheItem<T>));
        }

        public async Task<CacheItem<T>> FetchAsync<T>(string key, TimeSpan? reconstructWindow, TimeSpan? expiration = null,
            TimeSpan? ttl = null)
        {

            if (_memoryCache.TryGetValue(key, out byte[] storedValue))
            {
                var value = _serializer.Deserialize<CacheItem<T>>(storedValue);

                if (!value.Expiration.HasValue || !value.IsExpired || !reconstructWindow.HasValue)
                    return value;

                var actualExpiration = value.Expiration.Value;

                value.ExpirationTicks = actualExpiration.Add(reconstructWindow.Value).Ticks;

                await StoreAsync(key, value.Data, expiration, ttl).ConfigureAwait(false);

                return new CacheItem<T>()
                {
                    ExpirationTicks = actualExpiration.Ticks,
                    Data = value.Data
                };
            }

            return default(CacheItem<T>);
        }

        public Task RemoveAsync(string key, TimeSpan? removeAt = null)
        {
            if (!removeAt.HasValue)
                _memoryCache.Remove(key);

            else if (_memoryCache.TryGetValue(key, out byte[] value))
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
