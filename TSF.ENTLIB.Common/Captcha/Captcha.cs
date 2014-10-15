// ===============================================================================
// 版权    ：TSINGFANG
// 创建时间：2011-11-1
// 作者    ：枢木 ideal35500@qq.com
// 文件    ：Captcha.cs
// 功能    ：验证码提供
// 说明    ：
// ===============================================================================

using System;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Security.Cryptography;

namespace TSF.ENTLIB.Common.Captcha
{
    /// <summary>
    /// 验证码图片接口
    /// </summary>
    public interface ICaptcha
    {
        /// <summary>
        /// 生成验证码图片
        /// </summary>
        /// <param name="code">要显示的验证码</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <param name="bgcolor">背景色</param>
        CaptchaInfo GenerateImage(string code, int width, int height, Color bgcolor);
    }

    /// <summary>
    /// 验证码图片类
    /// </summary>
    public class Captcha : ICaptcha
    {
        private static byte[] randb = new byte[4];
        private static RNGCryptoServiceProvider rand = new RNGCryptoServiceProvider();

        private static Font[] fonts = {
                                        new Font(new FontFamily("Times New Roman"), 16 + Next(5), FontStyle.Regular),
                                        new Font(new FontFamily("Georgia"), 16 + Next(5), FontStyle.Regular),
                                        new Font(new FontFamily("Arial"), 16 + Next(5), FontStyle.Regular)
                                     };
        /// <summary>
        /// 获得下一个随机数
        /// </summary>
        /// <param name="max">最大值</param>
        /// <returns></returns>
        private static int Next(int max)
        {
            rand.GetBytes(randb);
            int value = BitConverter.ToInt32(randb, 0);
            value = value % (max + 1);
            if (value < 0)
                value = -value;
            return value;
        }

        /// <summary>
        /// 获得下一个随机数
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns></returns>
        private static int Next(int min, int max)
        {
            int value = Next(max - min) + min;
            return value;
        }

        #region ICaptcha 成员

        public CaptchaInfo GenerateImage(string code, int width, int height, Color bgcolor)
        {
            CaptchaInfo captchaInfo = new CaptchaInfo();
            captchaInfo.ImageFormat = ImageFormat.Jpeg;
            captchaInfo.ContentType = "image/pjpeg";

            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(bitmap);
            g.SmoothingMode = SmoothingMode.HighSpeed;
            g.Clear(bgcolor);

            Pen linePen = new Pen(Color.FromArgb(Next(90) + 10, Next(90) + 10, Next(90) + 10), 1);
            for (int i = 0; i < 3; i++)
            {
                g.DrawArc(linePen, Next(50), Next(25), Next(width) + 10, Next(height) + 10, Next(-180, 90), Next(90, -180));
            }

            Bitmap charbmp = new Bitmap(50, 50);
            Graphics charg = Graphics.FromImage(charbmp);
            SolidBrush drawBrush = new SolidBrush(Color.FromArgb(Next(180), Next(180), Next(180)));
            float charx = -25;
            PointF drawPoint = new PointF(0, 0);

            for (int i = 0; i < code.Length; i++)
            {
                charg.Clear(Color.Transparent);
                drawBrush.Color = Color.FromArgb(Next(90), Next(90), Next(90));
                charx = charx + Next(25) + 25;
                drawPoint = new PointF(charx, 2.0F);

                charg.DrawString(code[i].ToString(), fonts[Next(fonts.Length - 1)], drawBrush, new PointF(0, 0));
                g.DrawImage(charbmp, drawPoint);
            }

            drawBrush.Dispose();
            charg.Dispose();
            g.Dispose();

            captchaInfo.Image = bitmap;

            return captchaInfo;
        }

        #endregion
    }
}