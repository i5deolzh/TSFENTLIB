// ===============================================================================
// 版权    ：枢木
// 创建时间：2011-7 修改：
// 作者    ：枢木 ideal35500@qq.com
// 文件    ：
// 功能    ：
// 说明    ：
// ===============================================================================

using System;
using System.IO;

namespace TSF.ENTLIB.Common.Util
{
    public static class FileUtil
    {
        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public static bool IsFileExists(string fileName)
        {
            return System.IO.File.Exists(fileName);
        }

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool CreateDirectory(string path)
        {
            return MakeSureDirectoryPathExists(path);
        }
        [System.Runtime.InteropServices.DllImport("dbgHelp", SetLastError = true)]
        private static extern bool MakeSureDirectoryPathExists(string path);

        //---------------------------------------------------------

        /// <summary>
        /// 备份文件
        /// </summary>
        /// <param name="srcFileName">源文件名</param>
        /// <param name="destFileName">目标文件名</param>
        /// <param name="overwrite">当目标文件存在时是否覆盖</param>
        /// <returns></returns>
        public static bool BackupFile(string srcFileName, string destFileName, bool overwrite)
        {
            if (!overwrite && File.Exists(destFileName))
                return false;

            try
            {
                File.Copy(srcFileName, destFileName, true);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        }

        //---------------------------------------------------------

        /// <summary>
        /// 恢复文件
        /// </summary>
        /// <param name="backupFileName">备份文件名</param>
        /// <param name="targetFileName">要恢复的文件名</param>
        /// <returns></returns>
        public static bool RestoreFile(string backupFileName, string targetFileName)
        {
            return RestoreFile(backupFileName, targetFileName, null);
        }

        /// <summary>
        /// 恢复文件
        /// </summary>
        /// <param name="backupFileName">备份文件名</param>
        /// <param name="targetFileName">要恢复的文件名</param>
        /// <param name="backupTargetFileName">要恢复文件再次备份的名称，如果为null，则不备份</param>
        /// <returns></returns>
        public static bool RestoreFile(string backupFileName, string targetFileName, string backupTargetFileName)
        {
            try
            {
                if (backupTargetFileName != null)
                {
                    if (!File.Exists(targetFileName))
                        throw new FileNotFoundException(targetFileName + "文件不存在，无法备份此文件！");
                    else
                        File.Copy(targetFileName, backupTargetFileName, true);
                }

                File.Delete(targetFileName);
                File.Copy(backupFileName, targetFileName);
            }
            catch (Exception e)
            {
                throw e;
            }

            return true;
        }

        //---------------------------------------------------------

        /// <summary>
        /// 获取文件名
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public static string GetFileName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName) || fileName.IndexOf('.') < 1)
                return "";

            return Path.GetFileName(fileName);
        }

        /// <summary>
        /// 获取文件的扩展名
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public static string GetFileExtName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName) || fileName.IndexOf('.') < 1)
                return "";

            return Path.GetExtension(fileName);
        }
    }
}
