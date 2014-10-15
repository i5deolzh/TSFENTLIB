using System;
using System.Configuration;

namespace TSF.ENTLIB.Common.Caching
{
    /// <summary>
    /// 缓存提供程序
    /// 支持：
    /// -    无缓存：NoCache
    /// -  默认提供：DefaultCache
    /// -    分布式：MemcachedClient
    /// 可在配置文件AppSettings配置节进行配置
    /// 如：<add key="Cache.Type" value="Memcached"/> 或 <add key="Cache.Type" value="NoCache"/>
    /// </summary>
    public class CacheProvider
    {
        static ICache _instance;

        static CacheProvider()
        {
            _instance = new DefaultCache();

            string cacheType = ConfigurationManager.AppSettings["Cache.Type"];
            if (!string.IsNullOrEmpty(cacheType))
            {
                switch (cacheType.ToLower())
                {
                    case "memcached":
                        _instance = new MemcachedClient();
                        break;
                    case "nocache":
                        _instance = new NoCache();
                        break;
                }
            }
        }

        public static ICache CurrentInstance
        {
            get { return _instance; }
        }
    }
}
