// ===============================================================================
// 版权    ：TSINGFANG
// 创建时间：2011-7-1
// 作者    ：枢木 ideal35500@qq.com
// 文件    ：SQLServerUtil.cs
// 功能    ：SQL Server 数据库辅助工具
// 说明    ：
// ===============================================================================

namespace TSF.ENTLIB.Data
{
    using System;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;

    /// <summary>
    /// SQL Server 数据库辅助工具
    /// </summary>
    public static class SQLServerUtil
    {
        #region methods

        /// <summary>
        /// 获取默认数据库连接字符串
        /// <remarks>
        /// 需要在应用程序配置文件中配置connectionStrings点，默认数据库连接字符串名称：DEFAULT_CONN_STRING
        /// </remarks>
        /// </summary>
        /// <returns></returns>
        public static string GetConnectionString()
        {
            return GetConnectionString("DEFAULT_CONN_STRING");
        }

        /// <summary>
        /// 获取数据库连接字符串
        /// </summary>
        /// <param name="ConnectionStringName">一个有效的数据库连接字符串名称</param>
        /// <returns></returns>
        public static string GetConnectionString(string ConnectionStringName)
        {
            string connectionString = null;

            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[ConnectionStringName];
            if (settings != null)
            {
                connectionString = settings.ConnectionString;
            }

            return connectionString;
        }

        /// <summary>
        /// 获取Master数据库连接字符串
        /// </summary>
        /// <param name="connetionString">一个有效的数据库连接字符串</param>
        /// <returns></returns>
        public static string GetMasterConnectionString(string connetionString)
        {
            var builder = new SqlConnectionStringBuilder(connetionString);
            builder.InitialCatalog = "master";
            return builder.ToString();
        }

        /// <summary>
        /// 获取数据库名称
        /// </summary>
        /// <param name="connetionString">一个有效的数据库连接字符串</param>
        /// <returns></returns>
        public static string GetDatabaseName(string connetionString)
        {
            var builder = new SqlConnectionStringBuilder(connetionString);
            return builder.InitialCatalog;
        }

        /// <summary>
        /// 创建SqlConnection实例
        /// </summary>
        /// <param name="connectionString">一个有效的数据库连接字符串</param>
        /// <returns></returns>
        public static SqlConnection CreateConnection(string connectionString)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            return conn;
        }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////        

        #region 创建数据库参数

        /// <summary>
        /// 创建数据库输入参数
        /// </summary>
        /// <param name="paramName">参数名称</param>
        /// <param name="dbType">参数数据类型</param>
        /// <param name="value">参数值</param>
        /// <returns></returns>
        public static SqlParameter AddInParameter(string paramName, SqlDbType dbType, object value)
        {
            return AddParameter(paramName, dbType, 0, ParameterDirection.Input, value);
        }

        /// <summary>
        /// 创建数据库输出参数
        /// </summary>
        /// <param name="paramName">参数名称</param>
        /// <param name="dbType">参数数据类型</param>
        /// <returns></returns>
        public static SqlParameter AddOutParameter(string paramName, SqlDbType dbType)
        {
            return AddParameter(paramName, dbType, 0, ParameterDirection.Output, null);
        }

        /// <summary>
        /// 创建数据库参数
        /// <remarks>
        /// 支持创建常用的数据库参数，如：输入、输出、返回等参数
        /// </remarks>
        /// </summary>
        /// <param name="paramName">参数名称</param>
        /// <param name="dbType">参数数据类型</param>
        /// <param name="size">长度</param>
        /// <param name="direction">参数类型：输入|输出|返回</param>
        /// <param name="value">参数值</param>
        /// <returns></returns>
        public static SqlParameter AddParameter(string paramName, SqlDbType dbType, int size, ParameterDirection direction, object value)
        {
            SqlParameter param;

            paramName = BuildParameterName(paramName);

            if (size > 0)
                param = new SqlParameter(paramName, dbType, size);
            else
                param = new SqlParameter(paramName, dbType);

            param.Direction = direction;
            if (!(direction == ParameterDirection.Output && value == null))
                param.Value = value;

            return param;
        }

        private static string BuildParameterName(string parmName)
        {
            char parmToken = '@';
            if (parmName[0] != parmToken)
            {
                return parmName.Insert(0, new string(parmToken, 1));
            }

            return parmName;
        }

        #endregion
    }
}
