using Caching.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CacheHelper
{
    public interface ICacheHelper
    {
        Task<T> FetchAsync<T>(
            string cacheKey,
            Func<Task<T>> getObjectFromStorageFunc,
            TimeSpan reconstructTime,
            TimeSpan cacheExpiration,
            TimeSpan ttl);

        Task RemoveCacheAsync(string cacheKey, TimeSpan? expiration);

        Task RemoveCacheAsync(string prefix,
            object cacheKey, TimeSpan? expiration);

        Task RemoveCacheAsync(string prefix,
            string cacheKey, TimeSpan? expiration);

        Task RemoveCacheAsync(string prefix,
            Guid cacheKey, TimeSpan? expiration);

        Task<T> FetchAsync<T>(
            string prefix,
            object cacheKey,
            Func<Task<T>> getObjectFromStorageFunc,
            TimeSpan reconstructTime,
            TimeSpan cacheExpiration,
            TimeSpan ttl);


        Task<T> FetchAsync<T>(
            string prefix,
            Guid cacheKey,
            Func<Task<T>> getObjectFromStorageFunc,
            TimeSpan reconstructTime,
            TimeSpan cacheExpiration,
            TimeSpan ttl);

        Task<T> FetchAsync<T>(
            string prefix,
            string cacheKey,
            Func<Task<T>> getObjectFromStorageFunc,
            TimeSpan reconstructTime,
            TimeSpan cacheExpiration,
            TimeSpan ttl);

        Task StorePlainTextAsync<T>(string key,
                T value,
                TimeSpan? expiration,
                TimeSpan? ttl);

        Task<T> FetchPlainTextAsync<T>(string cacheKey, 
                TimeSpan reconstructTime);


        Task StoreLocationAsync(string key,
                double latitude,
                double longitude,
                string prefixStatus,
                TimeSpan? expiration,
                TimeSpan? ttl);

        Task<List<PersonLocation>> GetLocationsAsync(
            string key, 
            double latitude, 
            double longitude, 
            double radius);
    }
}