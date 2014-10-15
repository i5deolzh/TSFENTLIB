using System;
using System.Collections.Generic;

using Enyim.Caching;
using Enyim.Caching.Memcached;

namespace TSF.ENTLIB.Common.Caching
{
    /// <summary>
    /// 缓存
    /// Memcached分布式支持
    /// </summary>
    class MemcachedClient : ICache
    {
        static Enyim.Caching.MemcachedClient mc = new Enyim.Caching.MemcachedClient();

        #region ICache 成员

        public T Get<T>(string key)
        {
            return mc.Get<T>(key);
        }
        public object Get(string key)
        {
            return mc.Get(key);
        }

        public bool Put(string key, object value)
        {
            return mc.Store(StoreMode.Set, key, value);
        }
        public bool Put(string key, object value, TimeSpan validFor)
        {
            return mc.Store(StoreMode.Set, key, value, validFor);
        }
        public bool Put(string key, object value, DateTime expiresAt)
        {
            return mc.Store(StoreMode.Set, key, value, expiresAt);
        }
        public bool Put(string key, object value, System.Web.Caching.CacheDependency dep)
        {
            throw new NotImplementedException("[MemcachedClient]No support System.Web.Caching.CacheDependency");
        }

        public bool Remove(string key)
        {
            return mc.Remove(key);
        }
        public void RemoveByPattern(string pattern) { }

        public void Clear() { }

        #endregion
    }
}
