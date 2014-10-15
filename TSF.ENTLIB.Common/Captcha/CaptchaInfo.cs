// ===============================================================================
// 版权    ：TSINGFANG
// 创建时间：2011-11-1
// 作者    ：枢木 ideal35500@qq.com
// 文件    ：CaptchaInfo.cs
// 功能    ：验证码提供
// 说明    ：
// ===============================================================================

using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace TSF.ENTLIB.Common.Captcha
{
    /// <summary>
    /// 验证码图片信息
    /// </summary>
    public class CaptchaInfo
    {
        private Bitmap image;
        private string contentType = "image/pjpeg";
        private ImageFormat imageFormat = ImageFormat.Jpeg;

        /// <summary>
        /// 生成出的图片
        /// </summary>
        public Bitmap Image
        {
            get { return image;}
            set { image = value; }
        }

        /// <summary>
        /// 输出的图片类型，如 image/pjpeg
        /// </summary>
        public string ContentType
        {
            get { return contentType; }
            set { contentType = value; }
        }

        /// <summary>
        /// 图片的格式
        /// </summary>
        public ImageFormat ImageFormat
        {
            get { return imageFormat;}
            set { imageFormat = value;}
        }
    }
}
