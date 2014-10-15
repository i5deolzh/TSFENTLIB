// ===============================================================================
// 版权    ：TSINGFANG
// 创建时间：2011-7-1
// 作者    ：枢木 ideal35500@qq.com
// 文件    ：TypeConverter.cs
// 功能    ：类型转换
// 说明    ：
// ===============================================================================

using System;
using System.Text.RegularExpressions;

namespace TSF.ENTLIB.Common.Util
{
    /// <summary>
    /// 类型转换
    /// </summary>
    public sealed class TypeConverter
    {
        //-----------------------------------------------
        public static bool ConvertToBool(object obj)
        {
            return Str2Bool(obj.ToString());
        }

        //-----------------------------------------------
        /// <summary>
        /// 字符串转布尔值
        /// </summary>
        /// <param name="sValue">需转换的字符串，若其值为“TRUE”、“YES”或“1”其中之一，转换成功</param>
        /// <returns></returns>
        public static bool Str2Bool(string sValue)
        {
            if (string.Compare(sValue, "TRUE", true) == 0 || string.Compare(sValue, "YES", true) == 0 || string.Compare(sValue, "1", true) == 0)
                return true;
            else
                return false;
        }

        //-----------------------------------------------
        public static int Obj2Int(object obj)
        {
            return Obj2Int(obj, 0);
        }

        public static int Obj2Int(object obj, int defValue)
        {
            if (obj != null)
                return Str2Int(obj.ToString(), defValue);

            return defValue;
        }

        //-----------------------------------------------
        public static int Str2Int(string sValue)
        {
            return Str2Int(sValue, 0);
        }

        public static int Str2Int(string sValue, int defValue)
        {
            if (string.IsNullOrEmpty(sValue) || sValue.Trim().Length >= 11 || !Regex.IsMatch(sValue.Trim(), @"^([-]|[0-9])?[0-9]+(\.\w*)?$"))
                return defValue;

            int ret;
            if (Int32.TryParse(sValue, out ret))
                return ret;

            return Convert.ToInt32(Str2Float(sValue, defValue));
        }

        //-----------------------------------------------
        public static float Obj2Float(object obj)
        {
            return Obj2Float(obj, 0);
        }

        public static float Obj2Float(object obj, float defValue)
        {
            if (obj == null)
                return defValue;

            return Str2Float(obj.ToString(), defValue);
        }

        //-----------------------------------------------
        public static float Str2Float(string sValue)
        {
            return Str2Float(sValue, 0);
        }

        public static float Str2Float(string sValue, float defValue)
        {
            if (sValue == null || sValue.Length > 10)
                return defValue;

            float ret = defValue;
            if (sValue != null)
            {
                bool IsFloat = Regex.IsMatch(sValue, @"^([-]|[0-9])?[0-9]+(\.\w*)?$");
                if (IsFloat)
                    float.TryParse(sValue, out ret);
            }

            return ret;
        }

        //-----------------------------------------------
        public static DateTime Obj2DateTime(object obj)
        {
            return Obj2DateTime(obj, DateTime.Now);
        }

        public static DateTime Obj2DateTime(object obj, DateTime defValue)
        {
            return Str2DateTime(obj.ToString(), defValue);
        }

        //-----------------------------------------------
        public static DateTime Str2DateTime(string str)
        {
            return Str2DateTime(str, DateTime.Now);
        }

        public static DateTime Str2DateTime(string str, DateTime defValue)
        {
            if (!string.IsNullOrEmpty(str))
            {
                DateTime dateTime;
                if (DateTime.TryParse(str, out dateTime))
                    return dateTime;
            }

            return defValue;
        }
    }
}