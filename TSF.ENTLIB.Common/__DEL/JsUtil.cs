// ===============================================================================
// 版权    ：TSINGFANG
// 创建时间：2011-9-15
// 作者    ：枢木 ideal35500@qq.com
// 文件    ：JsUtil.cs
// 功能    ：脚本工具
// 说明    ：
// ===============================================================================

using System;
using System.Text;
using System.Web.UI;

namespace TSF.ENTLIB.Common.Web.Util
{
    /// <summary>
    /// 脚本工具
    /// </summary>
    public static class JsUtil
    {
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
    }
}
