using System;
using System.Collections.Generic;

namespace TSF.ENTLIB.Common.Caching
{
    /// <summary>
    /// 缓存策略接口
    /// </summary>
    public interface ICache
    {
        T Get<T>(string key);
        object Get(string key);

        bool Put(string key, object value);
        bool Put(string key, object value, TimeSpan validFor);
        bool Put(string key, object value, DateTime expiresAt);
        bool Put(string key, object value, System.Web.Caching.CacheDependency dep);

        bool Remove(string key);
        void RemoveByPattern(string pattern);

        void Clear();
    }
}
