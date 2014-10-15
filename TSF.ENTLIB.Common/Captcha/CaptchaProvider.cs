// ===============================================================================
// 版权    ：TSINGFANG
// 创建时间：2011-11-1
// 作者    ：枢木 ideal35500@qq.com
// 文件    ：CaptchaProvider.cs
// 功能    ：验证码提供
// 说明    ：
// ===============================================================================

using System;
using TSF.ENTLIB.Common.Util;

namespace TSF.ENTLIB.Common.Captcha
{
    /// <summary>
    /// 验证码图片创建类
    /// </summary>
    public class CaptchaProvider
    {
        /// <summary>
        /// 获取验证码的类实例
        /// </summary>
        /// <returns></returns>
        public static ICaptcha GetInstance()
        {
            return Singleton<Captcha>.GetInstance();
        }
    }
}
