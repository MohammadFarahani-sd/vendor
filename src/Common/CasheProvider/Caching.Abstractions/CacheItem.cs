using System;

namespace Caching.Abstractions
{
    [Serializable]
    public class CacheItem<T> : ICacheItem,ICloneable
    {
        public CacheItem(T data, TimeSpan? expiration = null)
        {
            Data = data;
            if (expiration.HasValue)
                ExpirationTicks = DateTime.Now.Add(expiration.Value).Ticks;
        }

        public CacheItem()
        {
        }

        public long? ExpirationTicks { get; set; }

        public DateTime? Expiration => ExpirationTicks.HasValue
            ? new DateTime(ExpirationTicks.Value)
            : (DateTime?)null;

        public T Data { get; set; }

        public bool IsExpired => ExpirationTicks.HasValue
                                 && ExpirationTicks < DateTime.Now.Ticks;

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public interface ICacheItem
    {
        long? ExpirationTicks { get; set; }

        DateTime? Expiration { get; }
    }
}