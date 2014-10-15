// ===============================================================================
// 版权    ：枢木
// 创建时间：2011-7 修改：
// 作者    ：枢木 ideal35500@qq.com
// 文件    ：
// 功能    ：
// 说明    ：
// ===============================================================================

using System;

namespace TSF.ENTLIB.Common.Util
{
    /// <summary>
    /// 单例工厂
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class Singleton<T> where T : new()
    {
        private static T instance = new T();
        private static object locker = new object();

        private Singleton() { }

        /// <summary>
        /// 获取实例
        /// </summary>
        public static T GetInstance()
        {
            if (instance == null)
            {
                lock (locker)
                {
                    if (instance == null)
                    {
                        instance = new T();
                    }
                }
            }

            return instance;
        }

        /// <summary>
        /// 设置实例
        /// </summary>
        /// <param name="value"></param>
        public void SetInstance(T value)
        {
            instance = value;
        }

    }
}
