// ===============================================================================
// 版权    ：枢木
// 创建时间：2011-7 修改：
// 作者    ：枢木 ideal35500@qq.com
// 文件    ：
// 功能    ：
// 说明    ：
// ===============================================================================

using System;
using System.Text;
using System.Security.Cryptography;

namespace TSF.ENTLIB.Common.Util
{
    /// <summary>
    /// 加密、解密
    /// </summary>
    public static class EncryptUtil
    {
        #region 使用缺省密钥加密/解密字符串

        /// <summary>
        /// 使用缺省密钥加密字符串
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        public static string Encrypt(string original)
        {
            return Encrypt(original, "TSF-U-2011X");
        }

        /// <summary>
        /// 使用缺省密钥解密字符串
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        public static string Decrypt(string encrypted)
        {
            return Decrypt(encrypted, "TSF-U-2011X");
        }

        #endregion

        #region 使用指定密钥加密/解密字符串

        /// <summary>
        /// 使用指定密钥加密字符串
        /// </summary>
        /// <param name="original"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Encrypt(string original, string key)
        {
            byte[] buff = System.Text.Encoding.Default.GetBytes(original);
            byte[] kb = System.Text.Encoding.Default.GetBytes(key);

            return Convert.ToBase64String(Encrypt(buff, kb));
        }


        /// <summary>
        /// 使用指定密钥解密字符串
        /// </summary>
        /// <param name="original"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Decrypt(string encrypted, string key)
        {
            return Decrypt(encrypted, key, System.Text.Encoding.Default);
        }

        /// <summary>
        /// 使用指定密钥解密字符串
        /// </summary>
        /// <param name="encrypted"></param>
        /// <param name="key"></param>
        /// <param name="encoding">字符编码</param>
        /// <returns></returns>
        public static string Decrypt(string encrypted, string key, Encoding encoding)
        {
            byte[] buff = Convert.FromBase64String(encrypted);
            byte[] kb = System.Text.Encoding.Default.GetBytes(key);

            return encoding.GetString(Decrypt(buff, kb));
        }

        #endregion

        #region 使用缺省密钥加密/解密byte[]

        /// <summary>
        /// 使用缺省密钥加密 byte[]
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        public static byte[] Encrypt(byte[] original)
        {
            byte[] key = System.Text.Encoding.Default.GetBytes("TSF-U-2011X");
            return Encrypt(original, key);
        }

        /// <summary>
        /// 使用缺省密钥解密 byte[]
        /// </summary>
        /// <param name="encrypted"></param>
        /// <returns></returns>
        public static byte[] Decrypt(byte[] encrypted)
        {
            byte[] key = System.Text.Encoding.Default.GetBytes("TSF-U-2011X");
            return Decrypt(encrypted, key);
        }

        #endregion

        #region  使用指定密钥加密/解密byte[]

        /// <summary>
        /// 使用指定密钥加密 byte[]
        /// </summary>
        /// <param name="original"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static byte[] Encrypt(byte[] original, byte[] key)
        {
            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
            des.Key = _MakeMD5(key);
            des.Mode = CipherMode.ECB;

            return des.CreateEncryptor().TransformFinalBlock(original, 0, original.Length);
        }

        /// <summary>
        /// 使用指定密钥解密 byte[]
        /// </summary>
        /// <param name="encrypted"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static byte[] Decrypt(byte[] encrypted, byte[] key)
        {
            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
            des.Key = _MakeMD5(key);
            des.Mode = CipherMode.ECB;

            return des.CreateDecryptor().TransformFinalBlock(encrypted, 0, encrypted.Length);
        }

        private static byte[] _MakeMD5(byte[] original)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] key = md5.ComputeHash(original);
            return key;
        }

        #endregion

        /////////////////////////////////////////////////////////////////

        #region DES加密

        /// <summary>
        /// 使用默认密钥加密
        /// </summary>
        /// <param name="sText"></param>
        /// <returns></returns>
        public static string DESEncrypt(string sText)
        {
            return DESEncrypt(sText, "TSF-U-2011X");
        }

        /// <summary>
        /// 使用给定密钥加密
        /// </summary>
        /// <param name="sText"></param>
        /// <param name="sKey">密钥</param>
        /// <returns></returns>
        public static string DESEncrypt(string sText, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.Default.GetBytes(sText);

            des.Key = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            des.IV = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);

            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            StringBuilder ret = new StringBuilder();

            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }

            return ret.ToString();
        }

        #endregion

        #region DES解密

        /// <summary>
        /// 使用默认密钥解密
        /// </summary>
        /// <param name="sText"></param>
        /// <returns></returns>
        public static string DESDecrypt(string sText)
        {
            return DESDecrypt(sText, "TSF-U-2011X");
        }

        /// <summary>
        /// 使用给定密钥解密
        /// </summary>
        /// <param name="sText"></param>
        /// <param name="sKey"></param>
        /// <returns></returns>
        public static string DESDecrypt(string sText, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            int len = sText.Length / 2;
            byte[] inputByteArray = new byte[len];

            int x, i;
            for (x = 0; x < len; x++)
            {
                i = Convert.ToInt32(sText.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }

            des.Key = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            des.IV = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);

            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            return Encoding.Default.GetString(ms.ToArray());
        }

        #endregion
    }
}
