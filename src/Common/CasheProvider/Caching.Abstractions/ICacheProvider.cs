using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Caching.Abstractions
{
    public interface ICacheProvider
    {
        /// <summary>
        ///     Stores an item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiration">The timespan specifying object expiration.</param>
        /// <param name="ttl">Overrides the lifetime of the object in cache. If provided, 
        /// the object will not be destroyed after expiration, instead it will be destroyed after ttl.
        /// Use TimeSpan.MaxValue to keep the object in cache forever.</param>
        /// <returns></returns>
        Task StoreAsync<T>(string key, T value, TimeSpan? expiration = null, TimeSpan? ttl = null);

        /// <summary>
        /// Sets the expiration and ttl of a stored item
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expiration"></param>
        /// <param name="ttl"></param>
        /// <returns></returns>
        Task SetExpirationAsync<T>(string key, TimeSpan expiration, TimeSpan? ttl = null);


        /// <summary>
        /// Fetches an item from cache. Returns null if key doesn't exist.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<CacheItem<T>> FetchAsync<T>(string key);

        /// <summary>
        /// Fetches an item, if reconstructWindow is not null and the cached object is expired. 
        /// the cached item expiration is automatically updated.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="reconstructWindow"></param>
        /// <param name="expiration">The timespan specifying object expiration.</param>
        /// <param name="ttl">Overrides the lifetime of the object in cache. If provided 
        /// the object will not be destoryed after expiration, instead it will be destroyed after ttl.
        /// Use TimeSpan.MaxValue to keep the object in cache forever.</param>
        /// <returns></returns>
        Task<CacheItem<T>> FetchAsync<T>(string key, TimeSpan? reconstructWindow,
            TimeSpan? expiration = null,
            TimeSpan? ttl = null);

        /// <summary>
        /// Removes a key from cache
        /// </summary>
        /// <param name="key"></param>
        /// <param name="removeAt">If not null, the key will be removed after the provided value</param>
        /// <returns></returns>
        Task RemoveAsync(string key, TimeSpan? removeAt = null);


        /// <summary>
        /// store location in redis with key and status of prefix
        /// </summary>
        /// <param name="key"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="prefixStatus"></param>
        /// <param name="expiration"></param>
        /// <param name="ttl"></param>
        /// <returns></returns>
        Task AddLocationAsync(string key, double latitude, double longitude, string prefixStatus, TimeSpan? expiration = null, TimeSpan? ttl = null);


        /// <summary>
        /// getting list of ids that is near current location 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        Task<List<PersonLocation>> GetLocationsFilterdAsync(string key, double latitude, double longitude, double radius);
    }
}
