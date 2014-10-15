// ===============================================================================
// 版权    ：TSINGFANG
// 创建时间：2011-7-1
// 作者    ：枢木 ideal35500@qq.com
// 文件    ：SEOUtil.cs
// 功能    ：SEO、辅助工具
// 说明    ：
// ===============================================================================

using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace TSF.ENTLIB.Common.Web.Util
{
    /// <summary>
    /// SEO、辅助工具
    /// </summary>
    public sealed partial class SEOUtil
    {
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
        /// <param name="sName">名称</param>
        /// <param name="content">内容</param>
        public static void SetMeta(Page page, string sName, string content)
        {
            HtmlMeta meta;

            foreach (var control in page.Header.Controls)
            {
                if (control is HtmlMeta)
                {
                    meta = (HtmlMeta)control;
                    if (meta.Name.ToLower().Equals(sName.ToLower()))
                    {
                        meta.Content = content;
                        return;
                    }
                }
            }

            meta = new HtmlMeta();
            meta.Name = sName;
            meta.Content = content;
            page.Header.Controls.Add(meta);
        }

        /// <summary>
        /// 为页面添加内联CSS样式
        /// </summary>
        /// <param name="style">CSS样式，如".test { width:100px; }"</param>
        public void SetStyle(Page page, string style)
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
        public void SetStyleLink(Page page, string url)
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
    }
}
