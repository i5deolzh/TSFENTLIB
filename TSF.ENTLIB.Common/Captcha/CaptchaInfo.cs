// ===============================================================================
// ��Ȩ    ��TSINGFANG
// ����ʱ�䣺2011-11-1
// ����    ����ľ ideal35500@qq.com
// �ļ�    ��CaptchaInfo.cs
// ����    ����֤���ṩ
// ˵��    ��
// ===============================================================================

using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace TSF.ENTLIB.Common.Captcha
{
    /// <summary>
    /// ��֤��ͼƬ��Ϣ
    /// </summary>
    public class CaptchaInfo
    {
        private Bitmap image;
        private string contentType = "image/pjpeg";
        private ImageFormat imageFormat = ImageFormat.Jpeg;

        /// <summary>
        /// ���ɳ���ͼƬ
        /// </summary>
        public Bitmap Image
        {
            get { return image;}
            set { image = value; }
        }

        /// <summary>
        /// �����ͼƬ���ͣ��� image/pjpeg
        /// </summary>
        public string ContentType
        {
            get { return contentType; }
            set { contentType = value; }
        }

        /// <summary>
        /// ͼƬ�ĸ�ʽ
        /// </summary>
        public ImageFormat ImageFormat
        {
            get { return imageFormat;}
            set { imageFormat = value;}
        }
    }
}
