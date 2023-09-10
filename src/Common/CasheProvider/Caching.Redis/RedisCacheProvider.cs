using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Caching.Abstractions;
using Redis.StackExchange;
using Serializer;
using StackExchange.Redis;

namespace Caching.Redis
{
    public class RedisCacheProvider : ICacheProvider
    {
        private readonly IDatabase _writeDatabase;
        private readonly IDatabase _readDatabase;
        private readonly ISerializer _serializer;

        /// <summary>
        /// Initialized a RedisCacheProvider
        /// </summary>
        /// <param name="writeDatabase">database to write cache data</param>
        /// <param name="readDatabase">database to read cache data from. if null writeDatabase will be used</param>
        /// <param name="serializer"></param>
        public RedisCacheProvider(IDatabase writeDatabase,
            IDatabase readDatabase = null,
            ISerializer serializer = null)
        {
            _writeDatabase = writeDatabase;
            _readDatabase = readDatabase ?? writeDatabase;
            _serializer = serializer ?? Configuration.Default.Serializer;
        }

        public Task StoreAsync<T>(string key, T value, TimeSpan? expiration = null, TimeSpan? ttl = null)
        {
            var cacheItem = new CacheItem<T>(data: value, expiration: expiration);
            return HashSetAsync(key: key,
                cacheItem: cacheItem,
                expiration: expiration,
                ttl: ttl,
                isNullIndicator: "nullIndicator");
        }

        public Task SetExpirationAsync<T>(string key, TimeSpan expiration, TimeSpan? ttl = null)
        {
            Task task = _writeDatabase.HashSetAsync(key,
                new[]
                {
                    new HashEntry(
                        nameof(CacheItem<int>.ExpirationTicks),
                        DateTime.Now.Add(expiration).Ticks
                    )
                });

            return ttl.HasValue
                ? task.ContinueWith(t => _writeDatabase.KeyExpireAsync(key, ttl))
                : task;
        }

        public Task<CacheItem<T>> FetchAsync<T>(string key)
        {
            return _readDatabase.HashGetAsync<CacheItem<T>>(key, _serializer);
        }

        public Task<CacheItem<T>> FetchAsync<T>(string key,
            TimeSpan? reconstructWindow,
            TimeSpan? expiration = null,
            TimeSpan? ttl = null)
        {
            Task<CacheItem<T>> task = _readDatabase.HashGetAsync<CacheItem<T>>(key, _serializer);
            if (!reconstructWindow.HasValue)
                return task;

            return task.ContinueWith(async hashGetTask =>
                {
                    CacheItem<T> result = hashGetTask.Result;
                    if (result?.ExpirationTicks == null || !result.IsExpired)
                        return result;

                    await _writeDatabase.HashSetAsync(key,
                        hashField: nameof(CacheItem<T>.ExpirationTicks),
                        value: DateTime.Now.Add(reconstructWindow.Value).Ticks);

                    return result;
                }, TaskContinuationOptions.OnlyOnRanToCompletion)
                .Unwrap();
        }

        public Task RemoveAsync(string key, TimeSpan? removeAt = null)
        {
            return removeAt.HasValue
                ? _writeDatabase.KeyExpireAsync(key, removeAt.Value)
                : _writeDatabase.KeyDeleteAsync(key);
        }

        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        private Task HashSetAsync<T>(string key, CacheItem<T> cacheItem,
            TimeSpan? expiration = null, TimeSpan? ttl = null, string isNullIndicator = null)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (cacheItem == null) throw new ArgumentNullException(nameof(cacheItem));

            return _writeDatabase.HashSetAsync(key: key,
                serializer: _serializer,
                value: cacheItem,
                expiration: ttl.HasValue
                    ? ttl.Value == TimeSpan.MaxValue
                        ? null
                        : ttl
                    : expiration,
                isNullIndicator: isNullIndicator);
        }

        public Task AddLocationAsync(string key, double latitude, double longitude, string prefixStatus, TimeSpan? expiration = null, TimeSpan? ttl = null)
        {
            return _writeDatabase.GeoLocationSetAsync(
                key: key,
                latitude: latitude,
                longitude: longitude,
                prefixStatus: prefixStatus,
                expiration: expiration,
                ttl: ttl);
        }

        public async Task<List<PersonLocation>> GetLocationsFilterdAsync(string key, double latitude, double longitude, double radius)
        {
            var result = await _readDatabase.LocationGetItemsAsync(
                key: key,
                latitude: latitude,
                longitude: longitude,
                radius: radius);
            var items = result.Select(c => new PersonLocation()
            {
                Id = Guid.Parse(c.Member.ToString().Split("#").First()),
                Status = c.Member.ToString().Split("#")[1],
                Latitude = c.Position.Value.Latitude,
                Longitude = c.Position.Value.Longitude,
                Distance = (double)c.Distance
            }).ToList();

            var resultItems = items
                .GroupBy(c => c.Id)
                .Select(c => c.OrderBy(o => o.Distance)
                    .Select(t => new PersonLocation()
                    {
                        Id = t.Id,
                        Status = t.Status,
                        Latitude = t.Latitude,
                        Longitude = t.Longitude,
                        Distance = t.Distance,
                    })
                    .FirstOrDefault()).ToList();
            var itemSelected = resultItems.Where(c => c.Status == "Available" || c.Status == "CheckIn").ToList();

            return itemSelected;

        }
    }
}