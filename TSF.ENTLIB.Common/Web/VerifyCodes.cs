// ===============================================================================
// 版权    ：枢木
// 创建时间：2011-7 修改：
// 作者    ：枢木 ideal35500@qq.com
// 文件    ：
// 功能    ：
// 说明    ：
// ===============================================================================

using System;
using System.Web;
using System.Web.UI;

using TSF.ENTLIB.Common.Util;
using TSF.ENTLIB.Common.Captcha;

namespace TSF.ENTLIB.Common.Web
{
    /// <summary>
    /// 此类用于动态生成验证码图片
    /// 及验证码的验证
    /// ------------------------------
    /// 需Session支持  
    /// </summary>
    public class VerifyCodes
    {
        private Page pg;

        private VerifyCodes() { }

        public VerifyCodes(Page page)
        {
            this.pg = page;

            this.pg.Response.CacheControl = "private";
            this.pg.Response.Expires = 0;
            this.pg.Response.AddHeader("pragma", "no-cache");
        }

        /// <summary>
        /// 显示验证码图片
        /// </summary>
        public void ToImage()
        {
            string codes = Utils.GetRandCode(4);
            this.pg.Session.Add("VerifyCodes", codes);

            CaptchaInfo captcha = CaptchaProvider.GetInstance().GenerateImage(codes, 200, 40, System.Drawing.Color.WhiteSmoke);

            this.pg.Response.Clear();
            this.pg.Response.ContentType = captcha.ContentType;
            captcha.Image.Save(this.pg.Response.OutputStream, captcha.ImageFormat);
        }

        ~VerifyCodes()
        {
            this.pg.Dispose();
        }

        /////////////////////////////////////////////////////////////

        /// <summary>
        /// 校验用户输入的验证码
        /// </summary>
        /// <param name="codes"></param>
        /// <returns></returns>
        public static bool Validate(string codes)
        {
            if (HttpContext.Current == null)
                return false;
            if (HttpContext.Current.Session["VerifyCodes"] == null)
                return false;

            if (string.Compare(HttpContext.Current.Session["VerifyCodes"].ToString(), codes, true) == 0)
            {
                HttpContext.Current.Session["VerifyCodes"] = null;
                return true;
            }

            return false;
        }
    }
}
