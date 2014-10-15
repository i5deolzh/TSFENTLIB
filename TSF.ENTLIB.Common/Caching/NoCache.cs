using System;
using System.Collections.Generic;

namespace TSF.ENTLIB.Common.Caching
{
    /// <summary>
    /// 缓存策略
    /// 无缓存
    /// </summary>
    class NoCache : ICache
    {
        #region ICache 成员

        public T Get<T>(string key) { return default(T); }
        public object Get(string key) { return null; }

        public bool Put(string key, object value) { return true; }
        public bool Put(string key, object value, TimeSpan validFor) { return true; }
        public bool Put(string key, object value, DateTime expiresAt) { return true; }
        public bool Put(string key, object value, System.Web.Caching.CacheDependency dep) { return true; }

        public bool Remove(string key) { return true; }
        public void RemoveByPattern(string pattern) { }

        public void Clear() { }

        #endregion
    }
}
