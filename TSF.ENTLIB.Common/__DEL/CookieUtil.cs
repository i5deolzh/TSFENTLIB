// ===============================================================================
// 版权    ：TSINGFANG
// 创建时间：2011-7-1
// 作者    ：枢木 ideal35500@qq.com
// 文件    ：CookieUtil.cs
// 功能    ：Cookie工具
// 说明    ：
// ===============================================================================

using System;
using System.Web;

using TSF.ENTLIB.Common.Util;

namespace TSF.ENTLIB.Common.Web.Util
{
    /// <summary>
    /// Cookie工具
    /// </summary>
    public sealed partial class CookieUtil
    {
        #region 写入

        /// <summary>
        /// 写Cookie值
        /// 过期时间：1个月
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="value">值</param>
        public static void SetCookie(string name, string value)
        {
            SetCookie(name, value, DateTime.Now.AddMonths(1));
        }

        /// <summary>
        /// 写Cookie值
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="value">值</param>
        /// <param name="expires">过期时间</param>
        public static void SetCookie(string name, string value, DateTime expires)
        {
            SetCookie(name, null, value, expires);
        }

        /// <summary>
        /// 写Cookie值
        /// 过期时间：1个月
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public static void SetCookie(string name, string key, string value)
        {
            SetCookie(name, key, value, DateTime.Now.AddMonths(1));
        }

        /// <summary>
        /// 写Cookie值
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expires">过期时间</param>
        public static void SetCookie(string name, string key, string value, DateTime expires)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(value))
            {
                return;
            }

            HttpCookie cookie = HttpContext.Current.Request.Cookies[name];
            if (cookie == null)
            {
                cookie = new HttpCookie(name);
            }

            cookie.Expires = expires;

            value = HttpUtility.HtmlEncode(value);
            if (key == null)
            {
                cookie.Value = value;
            }
            else
            {
                cookie[key] = value;
            }

            HttpContext.Current.Response.AppendCookie(cookie);
        }

        #endregion

        #region 读取

        /// <summary>
        /// 读Cookie值
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>cookie值</returns>
        public static string GetCookie(string name)
        {
            return GetCookie(name, null);
        }

        /// <summary>
        /// 读Cookie值
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="key">键</param>
        /// <returns>cookie值</returns>
        public static string GetCookie(string name, string key)
        {
            string ret = string.Empty;
            HttpCookie cookie = HttpContext.Current.Request.Cookies[name];

            if (cookie == null)
            {
                return ret;
            }

            if (string.IsNullOrEmpty(key))
            {
                ret = cookie.Value;
            }
            else
            {
                ret = cookie[key];
            }

            return HttpUtility.HtmlDecode(ret);
        }

        #endregion

        #region 扩展

        /// <summary>
        /// Gets integer value from cookie
        /// </summary>
        /// <param name="name">Cookie name</param>
        /// <returns>Result</returns>
        public static int GetCookieInt(String name)
        {
            string ret = GetCookie(name);
            if (String.IsNullOrEmpty(ret))
            {
                return 0;
            }

            return Convert.ToInt32(ret);
        }

        /// <summary>
        /// Gets boolean value from cookie
        /// </summary>
        /// <param name="name">Cookie name</param>
        /// <returns>Result</returns>
        public static bool GetCookieBool(String name)
        {
            string ret = GetCookie(name);
            if (String.IsNullOrEmpty(ret))
            {
                return false;
            }

            ret = ret.ToUpperInvariant();
            return (ret == "TRUE" || ret == "YES" || ret == "1");
        }

        #endregion
    }
}
