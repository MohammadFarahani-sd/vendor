using System;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Redis.StackExchange
{
    public static class DatabaseLocationExtensions
    {
        public static async Task GeoLocationSetAsync(this IDatabase db, string key, double latitude, double longitude, string prefixStatus, TimeSpan? expiration = null, TimeSpan? ttl = null)
        {
            await db.KeyExpireAsync(key, ttl.HasValue
                  ? ttl.Value == TimeSpan.MaxValue
                      ? null
                      : ttl
                  : expiration);


            await db.GeoAddAsync(key: key, new GeoEntry(longitude, latitude, prefixStatus));
        }

        public static async Task<GeoRadiusResult[]> LocationGetItemsAsync(this IDatabase db, string key, double latitude, double longitude, double radius)
        {
            return await db.GeoRadiusAsync(key, longitude, latitude, radius);
        }
    }
}