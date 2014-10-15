// ===============================================================================
// 版权    ：枢木
// 创建时间：2011-7 修改：2013-8
// 作者    ：枢木 ideal35500@qq.com
// 文件    ：
// 功能    ：
// 说明    ：
// ===============================================================================

using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

using TSF.ENTLIB.Common.Util;


namespace TSF.ENTLIB.Common.Web
{
    /// <summary>
    /// Web通用工具类
    /// </summary>
    public static class WebUtils
    {
        #region [Web请求]

        /// <summary>
        /// 获取请求参数的值，支持GET和POST请求
        /// </summary>
        /// <param name="sName">参数</param>
        /// <returns></returns>
        public static string GetString(string sName)
        {
            return GetString(sName, false);
        }

        /// <summary>
        /// 获取请求参数的值，支持GET和POST请求
        /// </summary>
        /// <param name="sName">参数</param>
        /// <param name="sqlSafeCheck">是否进行SQL安全检查</param>
        /// <returns></returns>
        public static string GetString(string sName, bool sqlSafeCheck = false)
        {
            string ret = null;

            string httpMethod = GetHttpMethod();
            switch (httpMethod)
            {
                case "GET":
                    ret = HttpContext.Current.Request.QueryString[sName];
                    break;
                case "POST":
                    ret = HttpContext.Current.Request.Form[sName];
                    break;
            }

            //其它处理，UNDO

            if (!Utils.IsEmpty(ret))
            {
                ret = ret.Trim();
            }

            return ret;
        }

        /// <summary>
        /// 获取请求参数的整数值，支持GET和POST请求
        /// </summary>
        /// <param name="sName">参数</param>
        /// <returns></returns>
        public static int GetInt(string sName)
        {
            string str = GetString(sName, false);
            return Utils.ConvertToInt(str);
        }

        /// <summary>
        /// 获取请求参数的小数值，支持GET和POST请求
        /// </summary>
        /// <param name="sName">参数</param>
        /// <returns></returns>
        public static decimal GetDecimal(string sName)
        {
            decimal retValue = 0;

            string str = GetString(sName, false);

            if (!Utils.IsEmpty(str))
            {
                decimal.TryParse(str, out retValue);
            }

            return retValue;
        }

        /// <summary>
        /// 返回指定的服务器变量信息
        /// </summary>
        /// <param name="name">服务器变量名</param>
        /// <returns></returns>
        public static string GetServerVariables(string name)
        {
            string ret = "";

            if (HttpContext.Current.Request.ServerVariables[name] != null)
            {
                ret = HttpContext.Current.Request.ServerVariables[name].ToString();
            }

            return ret;
        }

        /// <summary>
        /// 返回表单或Url参数的总个数
        /// </summary>
        /// <returns></returns>
        public static int GetParamCount()
        {
            return HttpContext.Current.Request.Form.Count + HttpContext.Current.Request.QueryString.Count;
        }

        /// <summary>
        /// 获取请求的方式
        /// </summary>
        /// <returns>GET | POST</returns>
        public static string GetHttpMethod()
        {
            return HttpContext.Current.Request.HttpMethod;
        }

        /// <summary>
        /// 获取端口号
        /// </summary>
        /// <returns></returns>
        public static int GetPort()
        {
            if (HttpContext.Current.Request.Url.IsDefaultPort)
            {
                return 80;
            }

            return HttpContext.Current.Request.Url.Port;
        }

        /// <summary>
        /// 获取主机头
        /// </summary>
        /// <returns></returns>
        public static string GetHost()
        {
            return GetHost(false);
        }

        /// <summary>
        /// 获取主机头
        /// </summary>
        /// <param name="includePort">是否包括端口</param>
        /// <returns></returns>
        public static string GetHost(bool withPort)
        {
            string ret = "";

            if (HttpContext.Current.Request.Url.IsDefaultPort)
            {
                ret = HttpContext.Current.Request.Url.Host;
            }
            else
            {
                if (withPort)
                {
                    ret = string.Format("{0}:{1}", HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port.ToString());
                }
                else
                {
                    ret = HttpContext.Current.Request.Url.Host;
                }
            }

            return ret;
        }

        /// <summary>
        /// 获取客户端的 IP 主机地址
        /// </summary>
        /// <returns></returns>
        public static string GetIP()
        {
            string ret = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            if (string.IsNullOrEmpty(ret))
                ret = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (string.IsNullOrEmpty(ret))
                ret = HttpContext.Current.Request.UserHostAddress;

            return ret;
        }

        /// <summary>
        /// 获取网站首页地址
        /// </summary>
        /// <returns></returns>
        public static string GetSiteUrl()
        {
            bool useSSL = IsCurrentConnectionSecured();
            string ret = useSSL ? "https://" : "http://";

            ret = ret + GetHost(true);
            ret = ret + HttpContext.Current.Request.ApplicationPath;

            if (!ret.EndsWith("/"))
            {
                ret += "/";
            }

            return ret;
        }

        /// <summary>
        /// 获取当前页面原始的 URL
        /// 如：/products/productpage.aspx?productid=512000
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentRawUrl()
        {
            return HttpContext.Current.Request.RawUrl;
        }

        /// <summary>
        /// 获取当前页面完整的 URL 地址
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentUrl()
        {
            return GetCurrentUrl(true);
        }

        /// <summary>
        /// 获取当前页面完整的 URL 地址
        /// </summary>
        /// <param name="withQueryString">是否包括查询参数</param>
        /// <returns></returns>
        public static string GetCurrentUrl(bool withQueryString)
        {
            if (withQueryString)
            {
                return HttpContext.Current.Request.Url.ToString();
            }

            return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Path);
        }

        /// <summary>
        ///  获取链接到当前页面的 URL 地址（即上一个页面 URL）
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentUrlReferrer()
        {
            if (HttpContext.Current.Request.UrlReferrer != null)
            {
                return HttpContext.Current.Request.UrlReferrer.ToString();
            }

            return "";
        }

        /// <summary>
        /// 重新加载当前页面
        /// </summary>
        public static void ReloadCurrentPage()
        {
            string url = GetCurrentUrl();
            HttpContext.Current.Response.Redirect(url);
        }

        public static string GetEmailHostName(string email)
        {
            if (email.IndexOf("@") < 0)
            {
                return "";
            }

            return email.Substring(email.LastIndexOf("@")).ToLower();
        }

        /// <summary>
        /// 判断当前页面是否接收到了Post请求
        /// </summary>
        /// <returns></returns>
        public static bool IsPost()
        {
            return HttpContext.Current.Request.HttpMethod.Equals("POST");
        }

        /// <summary>
        /// 判断当前页面是否接收到了Get请求
        /// </summary>
        /// <returns></returns>
        public static bool IsGet()
        {
            return HttpContext.Current.Request.HttpMethod.Equals("GET");
        }

        /// <summary>
        /// 判断当前访问是否来自浏览器软件
        /// </summary>
        /// <returns></returns>
        public static bool IsBrowserGet()
        {
            string[] BrowserName = { "ie", "opera", "netscape", "mozilla", "konqueror", "firefox" };
            string curBrowser = HttpContext.Current.Request.Browser.Type.ToLower();

            for (int i = 0; i < BrowserName.Length; i++)
            {
                if (curBrowser.IndexOf(BrowserName[i]) >= 0)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 判断是否来自搜索引擎链接
        /// </summary>
        /// <returns></returns>
        public static bool IsSearchEnginesGet()
        {
            if (HttpContext.Current.Request.UrlReferrer == null)
                return false;

            string[] SearchEngine = { "google", "yahoo", "msn", "baidu", "sogou", "sohu", "sina", "163", "lycos", "tom", "yisou", "iask", "soso", "gougou", "zhongsou" };
            string tmpReferrer = HttpContext.Current.Request.UrlReferrer.ToString().ToLower();

            for (int i = 0; i < SearchEngine.Length; i++)
            {
                if (tmpReferrer.IndexOf(SearchEngine[i]) >= 0)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsCurrentConnectionSecured()
        {
            return HttpContext.Current.Request.IsSecureConnection;
        }

        /// <summary>
        /// 修改url参数
        /// </summary>
        /// <param name="url">...productpage.aspx?productid=512000&type=1</param>
        /// <param name="queryStringModification">productid=123456</param>
        /// <param name="targetLocationModification"></param>
        /// <returns>...productpage.aspx?productid=123456&type=1</returns>
        public static string ModifyQueryString(string url, string queryStringModification, string targetLocationModification)
        {
            string str = string.Empty;
            string str2 = string.Empty;

            if (url.Contains("#"))
            {
                str2 = url.Substring(url.IndexOf("#") + 1);
                url = url.Substring(0, url.IndexOf("#"));
            }
            if (url.Contains("?"))
            {
                str = url.Substring(url.IndexOf("?") + 1);
                url = url.Substring(0, url.IndexOf("?"));
            }

            if (!string.IsNullOrEmpty(queryStringModification))
            {
                if (!string.IsNullOrEmpty(str))
                {
                    var dictionary = new Dictionary<string, string>();

                    foreach (string str3 in str.Split(new char[] { '&' }))
                    {
                        if (!string.IsNullOrEmpty(str3))
                        {
                            string[] strArray = str3.Split(new char[] { '=' });
                            if (strArray.Length == 2)
                            {
                                dictionary[strArray[0]] = strArray[1];
                            }
                            else
                            {
                                dictionary[str3] = null;
                            }
                        }
                    }
                    foreach (string str4 in queryStringModification.Split(new char[] { '&' }))
                    {
                        if (!string.IsNullOrEmpty(str4))
                        {
                            string[] strArray2 = str4.Split(new char[] { '=' });
                            if (strArray2.Length == 2)
                            {
                                dictionary[strArray2[0]] = strArray2[1];
                            }
                            else
                            {
                                dictionary[str4] = null;
                            }
                        }
                    }

                    var builder = new StringBuilder();
                    foreach (string str5 in dictionary.Keys)
                    {
                        if (builder.Length > 0)
                        {
                            builder.Append("&");
                        }
                        builder.Append(str5);
                        if (dictionary[str5] != null)
                        {
                            builder.Append("=");
                            builder.Append(dictionary[str5]);
                        }
                    }
                    str = builder.ToString();
                }
                else
                {
                    str = queryStringModification;
                }
            }

            if (!string.IsNullOrEmpty(targetLocationModification))
            {
                str2 = targetLocationModification;
            }

            return (url + (string.IsNullOrEmpty(str) ? "" : ("?" + str)) + (string.IsNullOrEmpty(str2) ? "" : ("#" + str2)));
        }

        /// <summary>
        /// 删除url参数
        /// </summary>
        /// <param name="url"></param>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public static string RemoveQueryString(string url, string queryString)
        {
            string str = string.Empty;

            if (url.Contains("?"))
            {
                str = url.Substring(url.IndexOf("?") + 1);
                url = url.Substring(0, url.IndexOf("?"));
            }

            if (!string.IsNullOrEmpty(queryString))
            {
                if (!string.IsNullOrEmpty(str))
                {
                    Dictionary<string, string> dictionary = new Dictionary<string, string>();
                    foreach (string str3 in str.Split(new char[] { '&' }))
                    {
                        if (!string.IsNullOrEmpty(str3))
                        {
                            string[] strArray = str3.Split(new char[] { '=' });
                            if (strArray.Length == 2)
                            {
                                dictionary[strArray[0]] = strArray[1];
                            }
                            else
                            {
                                dictionary[str3] = null;
                            }
                        }
                    }

                    dictionary.Remove(queryString);

                    var builder = new StringBuilder();
                    foreach (string str5 in dictionary.Keys)
                    {
                        if (builder.Length > 0)
                        {
                            builder.Append("&");
                        }
                        builder.Append(str5);
                        if (dictionary[str5] != null)
                        {
                            builder.Append("=");
                            builder.Append(dictionary[str5]);
                        }
                    }

                    str = builder.ToString();
                }
            }

            return (url + (string.IsNullOrEmpty(str) ? "" : ("?" + str)));
        }

        /// <summary>
        /// 从HttpRequest中构建键值对集合
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <returns></returns>
        public static NameValueCollection GetNameValueCollection(HttpRequest request)
        {
            NameValueCollection nv = new NameValueCollection();
            foreach (string key in request.Form.AllKeys)
            {
                if (!nv.AllKeys.Contains(key))
                {
                    nv.Add(key, request[key]);
                }
            }

            foreach (string key in request.QueryString.AllKeys)
            {
                if (!nv.AllKeys.Contains(key))
                {
                    nv.Add(key, request[key]);
                }
            }

            return nv;
        }

        /// <summary>
        /// 从QueryString中构建键值对集合
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public static NameValueCollection GetNameValueCollection(string queryString)
        {
            NameValueCollection nv = new NameValueCollection();
            foreach (string str in queryString.Split('&'))
            {
                if (!string.IsNullOrEmpty(str))
                {
                    string[] strArray = str.Split(new char[] { '=' });
                    if (strArray.Length == 2)
                    {
                        if (!nv.AllKeys.Contains(strArray[0]))
                        {
                            nv.Add(strArray[0], strArray[1]);
                        }
                    }
                }
            }

            return nv;
        }

        #endregion

        #region [COOKIE操作]

        //====================================
        //写Cookie值

        /// <summary>
        /// 写Cookie值
        /// 过期时间：1天
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="value">值</param>
        public static void SetCookie(string name, string value)
        {
            SetCookie(name, value, DateTime.Now.AddDays(1));
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
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expires">过期时间</param>
        public static void SetCookie(string name, string key, string value, DateTime expires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[name];
            if (cookie == null)
            {
                cookie = new HttpCookie(name);
            }

            cookie.Expires = expires;

            value = HttpUtility.UrlEncode(value);
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

        //====================================
        //读Cookie值

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

            if (key == null)
            {
                ret = cookie.Value;
            }
            else
            {
                ret = cookie[key];
            }

            return HttpUtility.UrlDecode(ret);
        }

        #endregion

        #region [页面HttpEquiv & Style]

        /// <summary>
        /// 设置页面HttpEquiv
        /// <example>
        ///     SetMetaHttpEquiv(this, "refresh", "10;URL=http://www.demo.com/");
        /// </example>
        /// AS:<meta http-equiv="refresh" content="10;URL=http://www.demo.com/" />
        /// </summary>
        /// <param name="page"></param>
        /// <param name="httpEquiv"></param>
        /// <param name="content"></param>
        public static void SetMetaHttpEquiv(Page page, string httpEquiv, string content)
        {
            HtmlMeta meta = new HtmlMeta();
            if (page.Header.FindControl("meta" + httpEquiv) != null)
            {
                meta = (HtmlMeta)page.Header.FindControl("meta" + httpEquiv);
                meta.Content = content;
            }
            else
            {
                meta.ID = "meta" + httpEquiv;
                meta.HttpEquiv = httpEquiv;
                meta.Content = content;
                page.Header.Controls.Add(meta);
            }
        }

        /// <summary>
        /// 设置页面关键字
        /// AS:<meta name="keywords" content="关键字" />
        /// </summary>
        /// <param name="page"></param>
        /// <param name="content"></param>
        public static void SetMetaKeywords(Page page, string content)
        {
            SetMeta(page, "keywords", content);
        }

        /// <summary>
        /// 设置页面说明
        /// AS:<meta name="description" content="说明" />
        /// </summary>
        /// <param name="page"></param>
        /// <param name="content"></param>
        public static void SetMetaDescription(Page page, string content)
        {
            SetMeta(page, "description", content);
        }

        /// <summary>
        /// 设置页面meta
        /// AS:
        ///    <meta name="keywords" content="关键字" />
        ///    <meta name="description" content="说明" />
        ///    <meta http-equiv="refresh" content="10" />
        ///    <meta http-equiv="refresh" content="10;URL=http://www.demo.com/" />
        ///    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
        /// </summary>
        /// <param name="page">需要设置的页面对象</param>
        /// <param name="name">名称</param>
        /// <param name="content">内容</param>
        public static void SetMeta(Page page, string name, string content)
        {
            HtmlMeta meta;

            foreach (var control in page.Header.Controls)
            {
                if (control is HtmlMeta)
                {
                    meta = (HtmlMeta)control;
                    if (meta.Name.ToLower().Equals(name.ToLower()))
                    {
                        meta.Content = content;
                        return;
                    }
                }
            }

            meta = new HtmlMeta();
            meta.Name = name;
            meta.Content = content;
            page.Header.Controls.Add(meta);
        }

        /// <summary>
        /// 为页面添加内联CSS样式
        /// </summary>
        /// <param name="style">CSS样式，如".test { width:100px; }"</param>
        public static void SetStyle(Page page, string style)
        {
            HtmlGenericControl node = new HtmlGenericControl("style");
            node.Attributes.Add("type", "text/css");
            node.InnerText = style;
            page.Header.Controls.Add(node);
        }

        /// <summary>
        /// 为页面添加外部CSS样式
        /// </summary>
        /// <param name="url">CSS文件URL</param>
        public static void SetStyleLink(Page page, string url)
        {
            HtmlLink link = new HtmlLink();
            link.Attributes.Add("type", "text/css");
            link.Attributes.Add("rel", "stylesheet");
            link.Attributes.Add("href", url);

            page.Header.Controls.Add(link);
        }

        /// <summary>
        /// 设置页面标题
        /// </summary>
        /// <param name="page"></param>
        /// <param name="title"></param>
        /// <param name="overwriteExisting"></param>  
        public static void SetTitle(Page page, string title)
        {
            page.Title = title;
        }

        public static void SetRSSLink(Page page, string href, string title)
        {
            var link = new HtmlGenericControl("link");
            link.Attributes.Add("type", "application/rss+xml");
            link.Attributes.Add("rel", "alternate");
            link.Attributes.Add("href", href);
            link.Attributes.Add("title", title);

            page.Header.Controls.Add(link);
        }

        #endregion

        #region [Js]

        public static void Alert(string sMsg, Page page)
        {
            sMsg = sMsg.Replace("'", "\"");
            sMsg = sMsg.Replace(Environment.NewLine, "");

            page.ClientScript.RegisterStartupScript(typeof(string), "", "<script>alert('" + sMsg + "');</script>");
        }

        public static void Alert(string clientID, string sMsg, Page page)
        {
            StringBuilder builder = new StringBuilder("");

            builder.Append("<script>");
            builder.Append("\t\t alert('" + sMsg + "');");
            builder.Append("\t\t document.forms(0)." + clientID + ".focus();");
            builder.Append("</script>");

            page.ClientScript.RegisterStartupScript(typeof(string), "", builder.ToString());
        }

        public static void AlertClose(string sMsg, Page page)
        {
            sMsg = sMsg.Replace("'", "\"");
            sMsg = sMsg.Replace(Environment.NewLine, "");
            sMsg = sMsg.Replace("\r\n", "");

            page.ClientScript.RegisterStartupScript(typeof(string), "", "<script>alert('" + sMsg + "');self.close();</script>");
        }

        public static void AlertRedirect(string sMsg, string url, Page page)
        {
            sMsg = sMsg.Replace("'", "\"");
            sMsg = sMsg.Replace(Environment.NewLine, "");
            sMsg = sMsg.Replace("\r\n", "");

            page.ClientScript.RegisterStartupScript(typeof(string), "", "<script>alert('" + sMsg + "');this.location.href='" + url + "';</script>");
        }

        public static void Confirm(string sMsg, string clientID_OK, Page page)
        {
            StringBuilder builder = new StringBuilder("");

            builder.Append("<script>");
            builder.Append("\t if (confirm('" + sMsg + "')==true)");
            builder.Append("\t {");
            builder.Append("\t\t document.forms(0)." + clientID_OK + ".click();");
            builder.Append("\t }");
            builder.Append("</script>");

            page.ClientScript.RegisterStartupScript(typeof(string), "", builder.ToString());
        }

        public static void Confirm(string sMsg, string clientID_OK, string clientID_Cancel, Page page)
        {
            StringBuilder builder = new StringBuilder("");

            builder.Append("<script>");
            builder.Append("\t if (confirm('" + sMsg + "')==true)");
            builder.Append("\t {");
            builder.Append("\t\tdocument.forms(0)." + clientID_OK + ".click();");
            builder.Append("\t }");
            builder.Append("\t else ");
            builder.Append("\t {");
            builder.Append("\t\t document.forms(0)." + clientID_Cancel + ".click();");
            builder.Append("\t }");
            builder.Append("</script>");

            page.ClientScript.RegisterStartupScript(typeof(string), "", builder.ToString());
        }

        public static void SetFocus(string clientID, Page page)
        {
            StringBuilder builder = new StringBuilder("");

            builder.Append("<script>");
            builder.Append("\t\t document.forms(0)." + clientID + ".focus(); ");
            builder.Append("\t\t document.forms(0)." + clientID + ".select(); ");
            builder.Append("</script>");

            page.ClientScript.RegisterStartupScript(typeof(string), "", builder.ToString());
        }

        #endregion

        #region [其他]

        private static AspNetHostingPermissionLevel? _trustLevel = null;
        /// <summary>
        /// Finds the trust level of the running application (http://blogs.msdn.com/dmitryr/archive/2007/01/23/finding-out-the-current-trust-level-in-asp-net.aspx)
        /// </summary>
        /// <returns>The current trust level.</returns>
        public static AspNetHostingPermissionLevel GetTrustLevel()
        {
            if (!_trustLevel.HasValue)
            {
                //set minimum
                _trustLevel = AspNetHostingPermissionLevel.None;

                //determine maximum
                foreach (AspNetHostingPermissionLevel trustLevel in new AspNetHostingPermissionLevel[] { AspNetHostingPermissionLevel.Unrestricted, AspNetHostingPermissionLevel.High, AspNetHostingPermissionLevel.Medium, AspNetHostingPermissionLevel.Low, AspNetHostingPermissionLevel.Minimal })
                {
                    try
                    {
                        new AspNetHostingPermission(trustLevel).Demand();
                        _trustLevel = trustLevel;
                        break; //we've set the highest permission we can
                    }
                    catch (System.Security.SecurityException)
                    {
                        continue;
                    }
                }
            }
            return _trustLevel.Value;
        }

        /// <summary>
        /// Restart application domain
        /// </summary>
        /// <param name="redirectUrl">Redirect URL; empty string if you want to redirect to the current page URL</param>
        public static void RestartAppDomain(string redirectUrl)
        {
            if (GetTrustLevel() > AspNetHostingPermissionLevel.Medium)
            {
                //full trust
                HttpRuntime.UnloadAppDomain();

                TryWriteGlobalAsax();
            }
            else
            {
                //medium trust
                bool success = TryWriteWebConfig();
                if (!success)
                {
                    throw new Exception("Needs to be restarted due to a configuration change, but was unable to do so.\r\n" +
                        "To prevent this issue in the future, a change to the web server configuration is required:\r\n" +
                        "- run the application in a full trust environment, or\r\n" +
                        "- give the application write access to the 'web.config' file.");
                }

                success = TryWriteGlobalAsax();
                if (!success)
                {
                    throw new Exception("Needs to be restarted due to a configuration change, but was unable to do so.\r\n" +
                        "To prevent this issue in the future, a change to the web server configuration is required:\r\n" +
                        "- run the application in a full trust environment, or\r\n" +
                        "- give the application write access to the 'Global.asax' file.");
                }
            }

            // If setting up extensions/modules requires an AppDomain restart, it's very unlikely the
            // current request can be processed correctly.  So, we redirect to the same URL, so that the
            // new request will come to the newly started AppDomain.
            var httpContext = HttpContext.Current;
            if (httpContext != null)
            {
                if (String.IsNullOrEmpty(redirectUrl))
                    redirectUrl = GetCurrentUrl(true);
                httpContext.Response.Redirect(redirectUrl, true);
            }
        }

        private static bool TryWriteWebConfig()
        {
            try
            {
                System.IO.File.SetLastWriteTimeUtc(Utils.GetMapPath("~/web.config"), DateTime.UtcNow);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static bool TryWriteGlobalAsax()
        {
            try
            {
                System.IO.File.SetLastWriteTimeUtc(Utils.GetMapPath("~/global.asax"), DateTime.UtcNow);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 从HTML中获取文本，保留br p img
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string GetTextFromHTML(string html)
        {
            Regex regEx = new Regex(@"</?(?!br|/?p|img)[^>]*>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            return regEx.Replace(html, "");
        }

        /// <summary>
        /// 移除HTML标签
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string RemoveHtml(string html)
        {
            return Regex.Replace(html, @"<[^>]*>", string.Empty, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 过滤HTML中的不安全标签
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string RemoveUnsafeHtml(string html)
        {
            html = Regex.Replace(html, @"(\<|\s+)o([a-z]+\s?=)", "$1$2", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"(script|frame|form|meta|behavior|style)([\s|:|>])+", "$1.$2", RegexOptions.IgnoreCase);

            return html;
        }

        public static string InputText(string text, int maxLen)
        {
            text = text.Trim();

            if (string.IsNullOrEmpty(text))
                return "";

            if (text.Length > maxLen)
                text = text.Substring(0, maxLen);

            text = Regex.Replace(text, "[\\s]{2,}", " ");	                            //two or more spaces
            text = Regex.Replace(text, "(<[b|B][r|R]/*>)+|(<[p|P](.|\\n)*?>)", "\n");	//<br>
            text = Regex.Replace(text, "(\\s*&[n|N][b|B][s|S][p|P];\\s*)+", " ");	    //&nbsp;
            text = Regex.Replace(text, "<(.|\\n)*?>", string.Empty);	                //any other tags
            text = text.Replace("'", "''");

            return text;
        }

        public static string ConvertTextToHtml(string text)
        {
            text = text.Replace("\r\n", "<br />");
            text = text.Replace("\r", "<br />");
            text = text.Replace("\n", "<br />");
            text = text.Replace("\t", "&nbsp;&nbsp;");
            text = text.Replace("  ", "&nbsp;&nbsp;");

            return text;
        }

        public static string ConvertHtmlToText(string text)
        {
            text = text.Replace("<br>", "\n");
            text = text.Replace("<br >", "\n");
            text = text.Replace("<br />", "\n");
            text = text.Replace("&nbsp;&nbsp;", "\t");
            text = text.Replace("&nbsp;&nbsp;", "  ");

            return text;
        }

        public static string FormatTextToP(string text)
        {
            if (String.IsNullOrEmpty(text))
                return "";

            Regex pStartRegex = new Regex("<p>", RegexOptions.IgnoreCase);
            Regex pEndRegex = new Regex("</p>", RegexOptions.IgnoreCase);

            text = pStartRegex.Replace(text, "");
            text = pEndRegex.Replace(text, "\n");
            text = text.Replace("\r\n", "\n").Replace("\r", "\n");
            text = text + "\n\n";
            text = text.Replace("\n\n", "\n");

            var strArray = text.Split(new char[] { '\n' });
            var builder = new StringBuilder();

            foreach (string str in strArray)
            {
                if ((str != null) && (str.Trim().Length > 0))
                {
                    builder.AppendFormat("<p>{0}</p>\n", str);
                }
            }

            return builder.ToString();
        }

        public static string CleanNonWord(string text)
        {
            return Regex.Replace(text, "\\W", "");
        }

        /// <summary>
        /// 返回 HTML 字符串的编码结果
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static string HtmlEncode(string str)
        {
            return HttpUtility.HtmlEncode(str);
        }

        /// <summary>
        /// 返回 HTML 字符串的解码结果
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static string HtmlDecode(string str)
        {
            return HttpUtility.HtmlDecode(str);
        }

        /// <summary>
        /// 返回 URL 字符串的编码结果
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static string UrlEncode(string str)
        {
            return HttpUtility.UrlEncode(str);
        }

        /// <summary>
        /// 返回 URL 字符串的编码结果
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static string UrlDecode(string str)
        {
            return HttpUtility.UrlDecode(str);
        }

        /// <summary>
        /// 判断指定IP是否在指定的IP数组的范围内, IP数组内的IP地址可以使用*表示该IP段任意, 例如192.168.1.*
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="ipArray"></param>
        /// <returns></returns>
        public static bool IsInIPArray(string ip, string[] ipArray)
        {
            string[] userIp = Utils.SplitString(ip, @".");

            for (int ipIndex = 0; ipIndex < ipArray.Length; ipIndex++)
            {
                string[] tmpIp = Utils.SplitString(ipArray[ipIndex], @".");
                int r = 0;

                for (int i = 0; i < tmpIp.Length; i++)
                {
                    if (tmpIp[i] == "*")
                        return true;

                    if (userIp.Length > i)
                    {
                        if (tmpIp[i] == userIp[i])
                            r++;
                        else
                            break;
                    }
                    else
                        break;
                }

                if (r == 4)
                    return true;
            }

            return false;
        }

        public static string EnsureOnlyAllowedHtml(string text)
        {
            string allowedTags = "br,hr,b,i,u,a,div,ol,ul,li,blockquote,img,span,p,em,strong,font,pre,h1,h2,h3,h4,h5,h6,address,ciate";

            var options = RegexOptions.IgnoreCase;
            var m = Regex.Matches(text, "<.*?>", options);
            for (int i = m.Count - 1; i >= 0; i--)
            {
                string tag = text.Substring(m[i].Index + 1, m[i].Length - 1).Trim().ToLower();

                if (!IsValidTag(tag, allowedTags))
                {
                    text = text.Remove(m[i].Index, m[i].Length);
                }
            }

            return text;
        }

        public static bool IsValidTag(string tag, string sAllowedTags)
        {
            string[] allowedTags = sAllowedTags.Split(',');

            if (tag.IndexOf("javascript") >= 0) return false;
            if (tag.IndexOf("vbscript") >= 0) return false;
            if (tag.IndexOf("onclick") >= 0) return false;

            char[] endchars = new char[] { ' ', '>', '/', '\t' };

            int pos = tag.IndexOfAny(endchars, 1);

            if (pos > 0) tag = tag.Substring(0, pos);
            if (tag[0] == '/') tag = tag.Substring(1);

            foreach (string aTag in allowedTags)
            {
                if (tag == aTag) return true;
            }

            return false;
        }

        public static string StripTags(string text)
        {
            text = Regex.Replace(text, @"(>)(\r|\n)*(<)", "><");
            text = Regex.Replace(text, "(<[^>]*>)([^<]*)", "$2");
            text = Regex.Replace(text, "(&#x?[0-9]{2,4};|&quot;|&amp;|&nbsp;|&lt;|&gt;|&euro;|&copy;|&reg;|&permil;|&Dagger;|&dagger;|&lsaquo;|&rsaquo;|&bdquo;|&rdquo;|&ldquo;|&sbquo;|&rsquo;|&lsquo;|&mdash;|&ndash;|&rlm;|&lrm;|&zwj;|&zwnj;|&thinsp;|&emsp;|&ensp;|&tilde;|&circ;|&Yuml;|&scaron;|&Scaron;)", "@");

            return text;
        }

        /// <summary>
        /// 转换为静态HTML
        /// </summary>
        public static void TransStaticHtml(string path, string outPath)
        {
            Page page = new Page();
            System.IO.StringWriter writer = new System.IO.StringWriter();
            page.Server.Execute(path, writer);

            System.IO.FileStream fs;

            if (System.IO.File.Exists(page.Server.MapPath("") + "\\" + outPath))
            {
                System.IO.File.Delete(page.Server.MapPath("") + "\\" + outPath);
                fs = System.IO.File.Create(page.Server.MapPath("") + "\\" + outPath);
            }
            else
            {
                fs = System.IO.File.Create(page.Server.MapPath("") + "\\" + outPath);
            }

            byte[] bt = Encoding.Default.GetBytes(writer.ToString());

            fs.Write(bt, 0, bt.Length);
            fs.Close();
        }

        /// <summary>
        /// 根据Url获取源文件内容
        /// </summary>
        /// <param name="url">合法的Url地址</param>
        /// <returns></returns>
        public static string GetSourceTextByUrl(string url)
        {
            WebRequest request = WebRequest.Create(url);
            request.Timeout = 10000; // 10秒超时

            WebResponse response = request.GetResponse();

            System.IO.Stream resStream = response.GetResponseStream();
            System.IO.StreamReader sr = new System.IO.StreamReader(resStream, Encoding.Default);

            return sr.ReadToEnd();
        }

        /// <summary>
        /// http POST请求url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetHttpWebResponse(string url)
        {
            return GetHttpWebResponse(url, "POST", string.Empty);
        }

        /// <summary>
        /// http POST请求url
        /// </summary>
        /// <param name="apiUrl"></param>
        /// <param name="method_name"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        private static string GetHttpWebResponse(string apiUrl, string postMethod, string postData)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(apiUrl);
            request.Method = postMethod;
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postData.Length;
            request.Timeout = 10000;

            HttpWebResponse response = null;

            try
            {
                System.IO.StreamWriter swRequestWriter = new System.IO.StreamWriter(request.GetRequestStream());
                swRequestWriter.Write(postData);
                if (swRequestWriter != null)
                    swRequestWriter.Close();

                response = (HttpWebResponse)request.GetResponse();
                using (System.IO.StreamReader reader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
            finally
            {
                if (response != null)
                    response.Close();
            }
        }

        /// <summary>
        /// Write XML to response
        /// </summary>
        public static void WriteResponseXML(string xml, string filename)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(xml);

            XmlDeclaration decl = document.FirstChild as XmlDeclaration;
            if (decl != null)
            {
                decl.Encoding = "utf-8";
            }

            byte[] data = Encoding.UTF8.GetBytes(document.InnerXml);
            WriteResponse(null, "text/xml", filename, data);
        }

        /// <summary>
        /// Write text to response
        /// </summary>
        public static void WriteResponseTXT(string txt, string filename)
        {
            byte[] data = Encoding.UTF8.GetBytes(txt);
            WriteResponse(null, "text/plain", filename, data);
        }

        /// <summary>
        /// Write XLS file to response
        /// </summary>
        public static void WriteResponseXLS(string filePath, string targetFileName)
        {
            byte[] data = System.IO.File.ReadAllBytes(filePath);
            WriteResponse(null, "text/xls", targetFileName, data);
        }

        /// <summary>
        /// Write PDF file to response
        /// </summary>
        public static void WriteResponsePDF(string filePath, string targetFileName)
        {
            byte[] data = System.IO.File.ReadAllBytes(filePath);
            WriteResponse(null, "text/pdf", targetFileName, data);
        }

        /// <summary>
        /// Write file to response
        /// </summary>
        public static void WriteResponse(string charset, string contentType, string filename, byte[] data)
        {
            if (string.IsNullOrEmpty(charset))
            {
                charset = "utf-8";
            }

            HttpResponse response = HttpContext.Current.Response;
            response.Clear();
            response.Charset = charset;
            response.ContentType = contentType;
            response.AddHeader("content-disposition", string.Format("attachment; filename={0}", filename));
            response.BinaryWrite(data);
            response.End();
        }

        #endregion
    }
}
