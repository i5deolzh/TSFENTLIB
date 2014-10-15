// ===============================================================================
// 版权    ：TSINGFANG
// 创建时间：2011-7-1
// 作者    ：枢木 ideal35500@qq.com
// 文件    ：CacheUtil.cs
// 功能    ：缓存工具
// 说明    ：
// ===============================================================================

using System;
using System.Web;
using System.Web.Caching;
using System.Collections;
using System.Text.RegularExpressions;

using TSF.ENTLIB.Common.Util;

namespace TSF.ENTLIB.Common.Caching
{
    /// <summary>
    /// 缓存策略
    /// 默认
    /// </summary>
    public class DefaultCache : ICache
    {
        static Cache _cache;

        static DefaultCache()
        {
            HttpContext current = HttpContext.Current;
            if (current != null)
                _cache = current.Cache;
            else
                _cache = HttpRuntime.Cache;
        }

        #region ICache 成员

        public T Get<T>(string key)
        {
            throw new NotImplementedException();
        }
        public object Get(string key)
        {
            return _cache[key];
        }

        public bool Put(string key, object value)
        {
            _cache.Insert(key, value);
            return true;
        }
        public bool Put(string key, object value, TimeSpan validFor)
        {
            _cache.Insert(key, value, null, DateTime.UtcNow, validFor);
            return true;
        }
        public bool Put(string key, object value, DateTime expiresAt)
        {
            _cache.Insert(key, value, null, expiresAt, TimeSpan.Zero);
            return true;
        }
        public bool Put(string key, object value, CacheDependency dep)
        {
            _cache.Insert(key, value, dep, DateTime.MaxValue, TimeSpan.Zero, CacheItemPriority.AboveNormal, null);
            return true;
        }

        public bool Remove(string key)
        {
            _cache.Remove(key);
            return true;
        }
        public void RemoveByPattern(string pattern)
        {
            IDictionaryEnumerator enumerator = _cache.GetEnumerator();
            Regex regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            while (enumerator.MoveNext())
            {
                if (regex.IsMatch(enumerator.Key.ToString()))
                {
                    _cache.Remove(enumerator.Key.ToString());
                }
            }
        }

        public void Clear()
        {
            IDictionaryEnumerator enumerator = _cache.GetEnumerator();
            while (enumerator.MoveNext())
            {
                _cache.Remove(enumerator.Key.ToString());
            }
        }

        #endregion
    }
}
