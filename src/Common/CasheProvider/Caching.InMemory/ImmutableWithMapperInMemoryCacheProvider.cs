using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Caching.Abstractions;
using System.Collections.Generic;

namespace Caching.InMemory
{
    /// <summary>
    /// Use AutoMapper to implement  Immutable in memory cache,All the type which will store in cache,Must be configure in AutoMapper configurations
    /// </summary>
    public class ImmutableWithMapperInMemoryCacheProvider : ICacheProvider
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IMapper _mapper;

        public ImmutableWithMapperInMemoryCacheProvider(IMemoryCache memoryCache, IMapper mapper)
        {
            _memoryCache = memoryCache;
            _mapper = mapper;
        }

        public Task StoreAsync<T>(string key, T value, TimeSpan? expiration = null, TimeSpan? ttl = null)
        {
            var cacheItem = new CacheItem<T>(_mapper.Map<T, T>(value), expiration);

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
                return Task.FromResult(new CacheItem<T>()
                {
                    Data = _mapper.Map<T, T>(value.Data),
                    ExpirationTicks = value.ExpirationTicks
                });

            return Task.FromResult(default(CacheItem<T>));
        }

        public Task<CacheItem<T>> FetchAsync<T>(string key, TimeSpan? reconstructWindow, TimeSpan? expiration = null,
            TimeSpan? ttl = null)
        {

            if (_memoryCache.TryGetValue(key, out CacheItem<T> value))
            {

                if (!value.Expiration.HasValue || !value.IsExpired || !reconstructWindow.HasValue)
                {

                    return Task.FromResult(new CacheItem<T>()
                    {
                        Data = _mapper.Map<T, T>(value.Data),
                        ExpirationTicks = value.ExpirationTicks
                    });
                }

                var actualExpiration = value.Expiration.Value;

                value.ExpirationTicks = actualExpiration.Add(reconstructWindow.Value).Ticks;

                if (ttl.HasValue)
                    _memoryCache.Set(key, value, DateTimeOffset.Now.Add(ttl.Value));
                else
                    _memoryCache.Set(key, value);

                return Task.FromResult(new CacheItem<T>()
                {
                    ExpirationTicks = actualExpiration.Ticks,
                    Data = _mapper.Map<T, T>(value.Data),
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
