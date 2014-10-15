// ===============================================================================
// 版权    ：枢木
// 创建时间：2011-7 修改：2013-8
// 作者    ：枢木 ideal35500@qq.com
// 文件    ：
// 功能    ：
// 说明    ：
// ===============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.IO;
using System.Data;

namespace TSF.ENTLIB.Common.Util
{
    /// <summary>
    /// 通用工具类
    /// </summary>
    public static class Utils
    {
        #region [验证]

        public readonly static Regex REG_IP = new Regex(@"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        public readonly static Regex REG_URL = new Regex(@"(http|https)\://([a-zA-Z0-9\.\-]+(\:[a-zA-Z0-9\.&%\$\-]+)*@)*((25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])|localhost|([a-zA-Z0-9\-]+\.)*[a-zA-Z0-9\-]+\.(com|edu|gov|int|mil|net|org|biz|arpa|info|sName|pro|aero|coop|museum|[a-zA-Z]{1,10}))(\:[0-9]+)*(/($|[a-zA-Z0-9\.\,\?\'\\\+&%\$#\=~_\-]+))*$");
        public readonly static Regex REG_EMAIL = new Regex(@"^[.\-_a-zA-Z0-9]+@[\-_a-zA-Z0-9]+\.[a-zA-Z0-9]+$");
        public readonly static Regex REG_TEL = new Regex(@"^(\d{11})|^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$");
        public readonly static Regex REG_MOBILE = new Regex(@"^(((\(\d{3}\))|(\d{3}\-))?13\d{9}|1\d{10})$");
        public readonly static Regex REG_ZIP = new Regex(@"^\d{6}$");
        public readonly static Regex REG_CNZH = new Regex(@"^[\u0391-\uFFE5]+$");
        public readonly static Regex REG_NUMBER = new Regex("^[0-9]+$");
        public readonly static Regex REG_NUMBER_SIGN = new Regex("^[+-]?[0-9]+$");
        public readonly static Regex REG_DECIMAL = new Regex("^[0-9]+[.]?[0-9]+$");
        public readonly static Regex REG_DECIMAL_SIGN = new Regex("^[+-]?[0-9]+[.]?[0-9]+$");
        public readonly static Regex REG_MONEY = new Regex("^[0-9]+[.]?[0-9]+$");

        /// <summary>
        /// 是否IP
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsIP(string str)
        {
            return REG_IP.IsMatch(str);
        }

        /// <summary>
        /// 是否Url
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsUrl(string str)
        {
            return REG_URL.IsMatch(str);
        }

        /// <summary>
        /// 是否EMail
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsEMail(string str)
        {
            return REG_EMAIL.IsMatch(str);
        }

        /// <summary>
        /// 是否Tel
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsTel(string str)
        {
            return REG_TEL.IsMatch(str);
        }

        /// <summary>
        /// 是否手机号码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsMobile(string str)
        {
            return REG_MOBILE.IsMatch(str);
        }

        /// <summary>
        /// 是否邮政编号
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsZip(string str)
        {
            return REG_ZIP.IsMatch(str);
        }

        /// <summary>
        /// 是否日期
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsDateTime(string str)
        {
            try
            {
                Convert.ToDateTime(str);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// 是否中文
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsCNZH(string str)
        {
            return REG_CNZH.IsMatch(str);
        }

        /// <summary>
        /// 是否整数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNumber(string str)
        {
            return REG_NUMBER.IsMatch(str);
        }

        /// <summary>
        /// 是否有符号整数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNumberSign(string str)
        {
            return REG_NUMBER_SIGN.IsMatch(str);
        }

        /// <summary>
        /// 是否Decimal
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsDecimal(string str)
        {
            return REG_DECIMAL.IsMatch(str);
        }

        /// <summary>
        /// 是否有符号Decimal
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsDecimalSign(string str)
        {
            return REG_DECIMAL_SIGN.IsMatch(str);
        }

        /// <summary>
        /// 是否Money
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsMoney(string str)
        {
            return REG_MONEY.IsMatch(str);
        }

        #endregion

        #region [类型转换]

        /// <summary>
        /// 字符串转换为数字类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int ConvertToInt(string str)
        {
            int ret;
            if (Int32.TryParse(str, out ret))
            {
                return ret;
            }
            return Convert.ToInt32(ConvertToFloat(str));
        }

        /// <summary>
        /// 字符串转换为浮点类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static float ConvertToFloat(string str)
        {
            float ret;
            if (float.TryParse(str, out ret))
            {
                return ret;
            }
            return ret;
        }

        /// <summary>
        /// 字符串转换为布尔类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool ConvertToBool(string str)
        {
            if (string.Compare(str, "TRUE", true) == 0 || string.Compare(str, "YES", true) == 0 || string.Compare(str, "OK", true) == 0 || string.Compare(str, "SUCCESS", true) == 0 || string.Compare(str, "1", true) == 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 字符串转换为时间类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static DateTime? ConvertToDateTime(string str)
        {
            DateTime ret;
            if (DateTime.TryParse(str, out ret))
            {
                return ret;
            }
            return ret;
        }

        #endregion

        #region [常用函数]

        /// <summary>
        /// 获取MD5
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetMD5(string str)
        {
            byte[] b = Encoding.UTF8.GetBytes(str);
            b = new System.Security.Cryptography.MD5CryptoServiceProvider().ComputeHash(b);

            string ret = "";
            for (int i = 0; i < b.Length; i++)
            {
                ret += b[i].ToString("x").PadLeft(2, '0');
            }

            return ret;
        }

        /// <summary>
        /// 获取SHA256
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetSHA256(string str)
        {
            byte[] b = Encoding.UTF8.GetBytes(str);
            b = new System.Security.Cryptography.SHA256Managed().ComputeHash(b);

            return Convert.ToBase64String(b);
        }

        /// <summary>
        /// 获取GUID
        /// </summary>
        public static string GetGUID()
        {
            return System.Guid.NewGuid().ToString();
        }

        public static string GetRandCode(int len)
        {
            char[] chrArray = new char[] { 
                'a', 'b', 'd', 'c', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'p', 'r', 
                'q', 's', 't', 'u', 'v', 'w', 'z', 'y', 'x', '0', '1', '2', '3', '4', '5', '6', 
                '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 
                'N', 'Q', 'P', 'R', 'T', 'S', 'V', 'U', 'W', 'X', 'Y', 'Z'
             };
            return GetRand(chrArray, len);
        }

        public static string GetRandLetter(int len)
        {
            char[] chrArray = new char[] { 
                'a', 'b', 'd', 'c', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'p', 'r', 
                'q', 's', 't', 'u', 'v', 'w', 'z', 'y', 'x', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 
                'H', 'I', 'J', 'K', 'L', 'M', 'N', 'Q', 'P', 'R', 'T', 'S', 'V', 'U', 'W', 'X', 
                'Y', 'Z'
             };
            return GetRand(chrArray, len);
        }

        /// <summary>
        /// 生成指定长度的数字随机码
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string GetRandNum(int len)
        {
            char[] chrArray = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            return GetRand(chrArray, len);
        }

        public static string GetRand(char[] chrArray, int len)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random(DateTime.Now.Millisecond);

            for (int i = 0; i < len; i++)
            {
                builder.Append(chrArray[random.Next(0, chrArray.Length)].ToString());
            }

            return builder.ToString();
        }

        /// <summary>
        /// 四舍五入，保留一位小数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal Round(decimal value)
        {
            return Round(value, 1);
        }

        /// <summary>
        /// 四舍五入
        /// </summary>
        /// <param name="value"></param>
        /// <param name="digits">保留小数位数</param>
        /// <returns></returns>
        public static decimal Round(decimal value, int digits)
        {
            decimal ret = value;
            bool negative = value < 0 ? true : false;

            if (negative)
            {
                ret = -1 * ret;
            }

            ret = System.Math.Round(value, digits, MidpointRounding.AwayFromZero);

            if (negative)
            {
                ret = -1 * ret;
            }

            return ret;
        }

        /// <summary>
        /// 检测是否有SQL危险字符
        /// </summary>
        /// <param name="str">要判断字符串</param>
        /// <returns></returns>
        public static bool IsSafeSqlString(string str)
        {
            return !Regex.IsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        }

        /// <summary>
        /// 清理字符串
        /// </summary>
        public static string CleanInput(string str)
        {
            return Regex.Replace(str, @"[^\w\.@-]", "");
        }

        /// <summary>
        /// 获取文件的真实后缀名。目前只支持JPG, GIF, PNG, BMP四种图片文件。
        /// </summary>
        /// <param name="fileData">文件字节流</param>
        /// <returns>JPG, GIF, PNG or null</returns>
        public static string GetFileSuffix(byte[] fileData)
        {
            if (fileData == null || fileData.Length < 10)
            {
                return null;
            }

            if (fileData[0] == 'G' && fileData[1] == 'I' && fileData[2] == 'F')
            {
                return "GIF";
            }
            else if (fileData[1] == 'P' && fileData[2] == 'N' && fileData[3] == 'G')
            {
                return "PNG";
            }
            else if (fileData[6] == 'J' && fileData[7] == 'F' && fileData[8] == 'I' && fileData[9] == 'F')
            {
                return "JPG";
            }
            else if (fileData[0] == 'B' && fileData[1] == 'M')
            {
                return "BMP";
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取文件的真实媒体类型。目前只支持JPG, GIF, PNG, BMP四种图片文件。
        /// </summary>
        /// <param name="fileData">文件字节流</param>
        /// <returns>媒体类型</returns>
        public static string GetMimeType(byte[] fileData)
        {
            string suffix = GetFileSuffix(fileData);
            string mimeType;

            switch (suffix)
            {
                case "JPG": mimeType = "image/jpeg"; break;
                case "GIF": mimeType = "image/gif"; break;
                case "PNG": mimeType = "image/png"; break;
                case "BMP": mimeType = "image/bmp"; break;
                default: mimeType = "application/octet-stream"; break;
            }

            return mimeType;
        }

        /// <summary>
        /// 根据文件后缀名获取文件的媒体类型。
        /// </summary>
        /// <param name="fileName">带后缀的文件名或文件全名</param>
        /// <returns>媒体类型</returns>
        public static string GetMimeType(string fileName)
        {
            string mimeType;
            fileName = fileName.ToLower();

            if (fileName.EndsWith(".bmp", StringComparison.CurrentCulture))
            {
                mimeType = "image/bmp";
            }
            else if (fileName.EndsWith(".gif", StringComparison.CurrentCulture))
            {
                mimeType = "image/gif";
            }
            else if (fileName.EndsWith(".jpg", StringComparison.CurrentCulture) || fileName.EndsWith(".jpeg", StringComparison.CurrentCulture))
            {
                mimeType = "image/jpeg";
            }
            else if (fileName.EndsWith(".png", StringComparison.CurrentCulture))
            {
                mimeType = "image/png";
            }
            else
            {
                mimeType = "application/octet-stream";
            }

            return mimeType;
        }

        #endregion

        #region [字符串操作]

        /// <summary>
        /// 对象是否为Null（或空）（如：集合[Count]、数组[Length]等于0）
        /// 支持字符串(string)、集合(继承ICollection)、数组(int[]、string[])和DataSet
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsEmpty(object obj)
        {
            if (obj == null) return true;

            if (obj is String)
            {
                return string.IsNullOrEmpty(obj.ToString());
            }
            else if (obj is ICollection)
                return ((ICollection)obj).Count == 0;
            else if (obj is Array)
                return ((Array)obj).Length == 0;
            else if (obj is System.Data.DataSet)
            {
                System.Data.DataSet ds = (System.Data.DataSet)obj;
                if (ds == null)
                {
                    return true;
                }
                if (ds.Tables.Count == 0)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 返回字符串真实长度, 1个汉字长度为2
        /// </summary>
        /// <returns></returns>
        public static int Len(string str)
        {
            return Encoding.Default.GetBytes(str).Length;
        }

        /// <summary>
        /// 删除字符串尾部的空格/换行/回车
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RTrim(string str)
        {
            //return str.TrimEnd(' ', '\r', '\n');

            for (int i = str.Length; i >= 0; i--)
            {
                if (str[i].Equals(" ") || str[i].Equals("\r") || str[i].Equals("\n"))
                {
                    str.Remove(i, 1);
                }
            }

            return str;
        }

        /// <summary>
        /// SQL危险字符串过滤
        /// exec|insert|delete|update|truncate|declare|drop|'
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FilterSql(string str)
        {
            string ret = str;

            string input = str.ToLower();

            Regex regx = new Regex(@"exec|insert|delete|update|truncate|declare|drop|'");
            MatchCollection mc = regx.Matches(input);

            string s;
            foreach (Match m in mc)
            {
                s = m.Value;
                ret = ret.Replace(s, "");

            }

            return ret;
        }

        /// <summary>
        /// 脏字替换
        /// </summary>
        /// <param name="str"></param>
        /// <param name="ban">
        /// 王八蛋=老好人
        /// 臭流氓=说谁呢？
        /// </param>
        /// <returns></returns>
        public static string FilterBan(string str, string ban)
        {
            string t1 = "", t2 = "";
            string[] sArray = SplitString(ban, "\r\n");

            for (int i = 0; i < sArray.Length; i++)
            {
                t1 = sArray[i].Substring(0, sArray[i].IndexOf("="));
                t2 = sArray[i].Substring(sArray[i].IndexOf("=") + 1);

                str = str.Replace(t1, t2);
            }

            return str;
        }

        /// <summary>
        /// 替换字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="search"></param>
        /// <param name="replace"></param>
        /// <returns></returns>
        public static string ReplaceString(string str, string search, string replace)
        {
            return Regex.Replace(str, Regex.Escape(search), replace, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 字符串转换为简体中文
        /// </summary>
        public static string ToSChinese(string str)
        {
            return Microsoft.VisualBasic.Strings.StrConv(str, Microsoft.VisualBasic.VbStrConv.SimplifiedChinese, 0);
        }

        /// <summary>
        /// 字符串转换为繁体中文
        /// </summary>
        public static string ToTChinese(string str)
        {
            return Microsoft.VisualBasic.Strings.StrConv(str, Microsoft.VisualBasic.VbStrConv.TraditionalChinese, 0);
        }

        /// <summary>
        /// 清除字符串数组中的重复项
        /// </summary>
        /// <param name="arr">字符串数组</param>
        /// <returns></returns>
        public static string[] DistinctStringArray(string[] arr)
        {
            Hashtable h = new Hashtable();

            foreach (string s in arr)
            {
                h[s.Trim()] = s;
            }

            string[] ret = new string[h.Count];
            h.Keys.CopyTo(ret, 0);

            return ret;
        }

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="str">被截取的字符串</param>
        /// <param name="len">指定长度</param>
        /// <param name="replace">用于替换的字符串</param>
        /// <returns></returns>
        public static string SubString(string str, int len, string replace)
        {
            string ret = str;

            byte[] b = Encoding.Default.GetBytes(str);

            if (b.Length > len)
            {
                int n = 0;
                for (int i = 0; i < len; i++)
                {
                    if (b[i] > 127)
                    {
                        n += 1;
                        if (n == 3)
                        {
                            n = 1;
                        }
                    }
                    else
                    {
                        n = 0;
                    }
                }

                if ((b[len - 1] > 127) && n == 1)
                {
                    len += 1;
                }

                byte[] b2 = new byte[len];
                Array.Copy(b, b2, len);

                ret = Encoding.Default.GetString(b2);
                ret = ret + replace;
            }

            return ret;
        }

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="len"></param>
        /// <param name="replace"></param>
        /// <returns></returns>
        public static string SubUnicodeString(string str, int len, string replace)
        {
            string ret = str;

            int byteLen = System.Text.Encoding.Default.GetByteCount(str);// 单字节字符长度
            int charLen = str.Length;                                    // 把字符平等对待时的字符串长度
            int byteCount = 0;                                           // 记录读取进度
            int pos = 0;                                                 // 记录截取位置

            if (byteLen > len)
            {
                for (int i = 0; i < charLen; i++)
                {
                    char chr = str.ToCharArray()[i];
                    int chrlen = Convert.ToInt32(chr);

                    if (chrlen > 255)  // 按中文字符计算加2
                        byteCount += 2;
                    else               // 按英文字符计算加1
                        byteCount += 1;

                    if (byteCount > len)
                    {
                        pos = i;
                        break;
                    }
                    else if (byteCount == len)
                    {
                        pos = i + 1;
                        break;
                    }
                }

                if (pos >= 0)
                    ret = str.Substring(0, pos) + replace;
            }
            else
                ret = str;

            return ret;
        }

        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="str">需要分割的字符串</param>
        /// <param name="split">分割符</param>
        /// <returns></returns>
        public static string[] SplitString(string str, string split)
        {
            return SplitString(str, split, false);
        }

        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="str">需要分割的字符串</param>
        /// <param name="split">分割符</param>
        /// <param name="isIgnoreRepeatItem">忽略重复项</param>
        /// <returns></returns>
        public static string[] SplitString(string str, string split, bool isIgnoreRepeatItem)
        {
            if (str.IndexOf(split) < 0)
                return new string[] { str };

            string[] result = Regex.Split(str, Regex.Escape(split), RegexOptions.IgnoreCase);

            if (isIgnoreRepeatItem)
            {
                result = DistinctStringArray(result);
            }

            return result;
        }

        /// <summary>
        /// 过滤字符串数组中每个元素为合适的大小
        /// 当长度小于minLen时，则忽略，-1为不限制最小长度
        /// 当长度大于maxLen时，取maxLen
        /// 如果数组中有null元素 ，则忽略null元素
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="minLen">单个元素最小长度</param>
        /// <param name="maxLen">单个元素最大长度</param>
        /// <returns></returns>
        public static string[] PadStringArray(string[] arr, int minLen, int maxLen)
        {
            int count = 0;

            for (int i = 0; i < arr.Length; i++)
            {
                if (minLen > -1 && arr[i].Length < minLen)
                {
                    arr[i] = null;
                    continue;
                }

                if (arr[i].Length > maxLen)
                    arr[i] = arr[i].Substring(0, maxLen);

                count++;
            }

            string[] ret = new string[count];
            for (int i = 0, j = 0; i < arr.Length && j < ret.Length; i++)
            {
                if (!string.IsNullOrEmpty(arr[i]))
                {
                    ret[j] = arr[i];
                    j++;
                }
            }

            return ret;
        }

        /// <summary>
        /// 判断字符串是否属于字符串数组中的元素
        /// </summary>
        /// <param name="search">字符串</param>
        /// <param name="arr">字符串数组</param>
        /// <param name="ignoreCase">是否区分大小写</param>
        /// <returns></returns>
        public static bool IsInArray(string search, string[] arr, bool ignoreCase)
        {
            return GetInArrayIndex(search, arr, ignoreCase) >= 0;
        }

        /// <summary>
        /// 获取字符串在字符串数组中的索引
        /// </summary>
        /// <param name="search">字符串</param>
        /// <param name="arr">字符串数组</param>
        /// <param name="ignoreCase">是否区分大小写</param>
        /// <returns>不存在返回-1</returns>
        public static int GetInArrayIndex(string search, string[] arr, bool ignoreCase)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if (string.Compare(search, arr[i], ignoreCase) == 0)
                {
                    return i;
                }
            }

            return -1;
        }

        #endregion

        #region [日期操作]

        /// <summary>
        /// 返回日期/时间
        /// <see cref="http://msdn.microsoft.com/zh-cn/library/8kb3ddd4.aspx"/>
        /// </summary>
        /// <param name="fmt"></param>
        /// <returns></returns>
        public static string GetDateTime(string fmt)
        {
            return FmtDateTime(DateTime.Now, fmt);
        }

        /// <summary>
        /// 格式日期/时间 
        /// </summary>
        /// <see cref="http://msdn.microsoft.com/zh-cn/library/8kb3ddd4.aspx"/>
        /// <param name="dateTime"></param>
        /// <param name="fmt"></param>
        /// <returns></returns>
        public static string FmtDateTime(DateTime dateTime, string fmt)
        {
            return dateTime.ToString(fmt);
        }

        #endregion

        #region [其它]

        /// <summary>
        /// 获取绝对路径
        /// </summary>
        /// <param name="path">相对路径，如：/uploadfiles/</param>
        /// <returns></returns>
        public static string GetMapPath(string path)
        {
            if (path.StartsWith("~/"))
            {
                path = path.Replace("~/", "/");
            }

            if (System.Web.HttpContext.Current != null)
            {
                return System.Web.HttpContext.Current.Server.MapPath(path);
            }
            else// 非web程序引用
            {
                path = path.Replace("/", "\\");
                if (path.StartsWith("\\"))
                {
                    path = path.Substring(path.IndexOf('\\', 1)).TrimStart('\\');
                }

                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
            }
        }

        public static bool IsNullOrDefault<T>(this T? value) where T : struct
        {
            return default(T).Equals(value.GetValueOrDefault());
        }

        public static T GetOneValue<T>(ref DataSet ds, string field)
        {
            return GetOneValue<T>(ref ds, 0, 0, field);
        }
        public static T GetOneValue<T>(ref DataSet ds, int table_index, string field)
        {
            return GetOneValue<T>(ref ds, table_index, 0, field);
        }
        public static T GetOneValue<T>(ref DataSet ds, int table_index, int row_index, string field)
        {
            T o = default(T);

            if (null == ds
                || ds.Tables.Count < (table_index + 1)
                || ds.Tables[table_index].Rows.Count < (row_index + 1)
                || typeof(System.DBNull) == ds.Tables[table_index].Rows[row_index][field].GetType())
            {
                return o;
            }

            o = (T)ds.Tables[table_index].Rows[row_index][field];

            return o;
        }

        #endregion
    }
}
