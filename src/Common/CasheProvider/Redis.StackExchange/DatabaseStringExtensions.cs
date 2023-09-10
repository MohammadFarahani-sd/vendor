using System;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Redis.StackExchange
{
    public static class DatabaseStringExtensions
    {
        public static Task StringSetAsync<T>(this IDatabase db, 
            string key, T value, TimeSpan? expiration = null)
        {
            return db.StringSetAsync(key,
                Configuration.Default.Serializer.Serialize(value),
                expiration);
        }

        public static Task<T> StringGetAsync<T>(this IDatabase db, string key)
        {
            return db.StringGetAsync(key)
                .ContinueWith(s => Configuration.Default.Serializer.Deserialize<T>(s.Result));
        }
    }
}