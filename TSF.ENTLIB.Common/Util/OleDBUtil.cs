// ===============================================================================
// 版权    ：枢木
// 创建时间：2011-7 修改：
// 作者    ：枢木 ideal35500@qq.com
// 文件    ：
// 功能    ：
// 说明    ：
// ===============================================================================

using System;
using System.Data;
using System.Data.OleDb;

/*
    File内容：
    库存数量	出版日期	书号	书名	定价	出版社
    1	2004	7887023440	律师视点（第二辑）	1680	人民大学
    2	2004	788702336X	未来之路：全球经济与金融体系中的中国	160	人民大学
    2	2004	7887023467	狼图腾CD	198	人民大学
    2	2005	9787887023575	闫锐敏硬笔书法示范教程	100	人民大学
    1	2005	9787887023612	构建和谐社会—郑功成教授演讲	200	人民大学
    ==========================================================
    string fileName;
    DataTable dt;
    DataTable dt2;

    fileName = "文件名.xls";
    dt= OleDBUtil.GetExcelTable(fileName);
    dt2 = Create();

    Validate(dt);            
    Import(dt, dt2);

    foreach (DataRow dr in dt2.Rows)
    {
        Console.WriteLine(string.Format("{0} {1} {2}", dr[0], dr[1], dr[2]));
    }
    ==========================================================
    static void Validate(DataTable dt)
    {
        OleDBUtil.ValidColumn(dt, typeof(string), "数量", "库存", "库存数量");
        OleDBUtil.ValidColumn(dt, typeof(string), "ISBN", "书号");
    }
    static DataTable Create()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("书号", typeof(string));
        dt.Columns.Add("书名", typeof(string));
        dt.Columns.Add("数量", typeof(int));

        return dt;
    }
    static void Import(DataTable sDT, DataTable tDT)
    {
        string Num = OleDBUtil.GetColumnName(sDT, "数量", "库存", "库存数量");
        string ISBN = OleDBUtil.GetColumnName(sDT, "ISBN", "书号");
        string Name = OleDBUtil.GetColumnName(sDT, "书名", "图书名称");

        foreach (DataRow dr in sDT.Rows)
        {
            DataRow dr2 = tDT.NewRow();

            dr2["书号"] = dr[ISBN];
            dr2["书名"] = dr[Name];
            dr2["数量"] = dr[Num];

            tDT.Rows.Add(dr2);
        }
    }
 */

namespace TSF.ENTLIB.Common.Util
{

    //操作 Excel，需要配置 Microsoft Excel 应用程序
    //配置步骤
    //-------------------------------------------------
    //控制面板>管理工具>组件服务>DCOM配置（展开“组件服务>计算机>我的电脑”）>Microsoft Excel Application>（鼠标右键）属性>选择“安全”面板
    //首先需要配置的是“启动和激活权限”：
    //自定义>编辑>增加“NETWORK SERVER”用户，选择“本地启动”和“本地激活”，确定
    //然后配置“访问权限”：
    //自定义>编辑>增加“NETWORK SERVER”用户,选择“本地访问”，确定
    //-------------------------------------------------
    //以上配置方式适用Windows Server 2003（经测试，Windows 7 配置类似）

    /// <summary>
    /// OLE DB 工具   
    /// </summary>
    /// 
    public class OleDBUtil
    {
        /// <summary>
        /// 获取Excel数据，必须带标题行，默认只读第一个sheet
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static DataTable GetExcelTable(string fileName)
        {
            return GetExcelTable(null, fileName);
        }

        /// <summary>
        /// 获取Excel数据
        /// </summary>
        /// <param name="sheetName"></param> 
        /// <param name="fileName"></param>       
        /// <returns></returns>
        public static DataTable GetExcelTable(string sheetName, string fileName)
        {
            if (string.IsNullOrEmpty(sheetName))
            {
                string[] names = GetExcelSheetNames(fileName);
                if (names == null)
                {
                    sheetName = "sheet1";
                }
                else
                {
                    sheetName = names[0];
                }
            }

            if (!sheetName.EndsWith("$"))
            {
                sheetName = sheetName + "$";
            }

            DataSet ds = null;
            string connString = string.Empty;
            string sql = "";

            connString = GetExcelConnString(fileName);
            sql = "SELECT * FROM [" + sheetName + "]";

            OleDbConnection conn = null;
            OleDbDataAdapter cmd = null;

            try
            {
                conn = new OleDbConnection(connString);
                conn.Open();

                cmd = new OleDbDataAdapter(sql, connString);
                ds = new DataSet();

                cmd.Fill(ds, "TABLE1");
            }
            catch (Exception ex)
            {
                string msg = "GetExcelTable：读取Excel出现错误\r\n";
                if (ex.Message.IndexOf("外部表不是预期的格式") > 0)
                {
                    msg += "非Excel2003格式\r\n";
                }

                throw new ApplicationException(msg + ex.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }

            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }

            return null;
        }

        /// <summary>
        /// 获取Excel数据
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="multiSheet"></param>
        /// <returns></returns>
        public static DataTable GetExcelTable(string fileName, bool multiSheet)
        {
            if (multiSheet)
            {
                string[] sheetNames;
                DataTable dataTable = null;

                sheetNames = GetExcelSheetNames(fileName);

                foreach (string sheet in sheetNames)
                {
                    if (string.IsNullOrEmpty(sheet))
                    {
                        continue;
                    }

                    DataTable dt = GetExcelTable(sheet, fileName);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        if (dataTable == null)
                        {
                            dataTable = new DataTable();
                            int i = 0;
                            while (i < dt.Columns.Count)
                            {
                                dataTable.Columns.Add(dt.Columns[i].ColumnName, typeof(string));
                                i++;
                            }
                        }

                        foreach (DataRow dr in dt.Rows)
                        {
                            DataRow dr1 = dataTable.NewRow();
                            if (dataTable.Columns.Count != dr.ItemArray.Length)
                            {
                                continue;
                            }

                            for (int i = 0; i < dr.ItemArray.Length; i++)
                            {
                                dr1[i] = dr[i].ToString();
                            }

                            dataTable.Rows.Add(dr1.ItemArray);
                        }
                    }
                }

                return dataTable;
            }
            else
            {
                return GetExcelTable(fileName);
            }
        }

        /// <summary>
        /// 获取Sheet名字
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string[] GetExcelSheetNames(string fileName)
        {
            Microsoft.Office.Interop.Excel.Application app = null;
            Microsoft.Office.Interop.Excel.Workbook wb = null;
            object objMiss = null;
            string[] sheetNames = null;

            objMiss = System.Reflection.Missing.Value;

            try
            {
                app = new Microsoft.Office.Interop.Excel.Application();
                app.DisplayAlerts = false;
                app.Visible = false;

                wb = app.Workbooks.Open(fileName, 1, true, objMiss, objMiss, objMiss, objMiss, objMiss, objMiss, objMiss, objMiss, objMiss, objMiss, objMiss, objMiss);
                sheetNames = new string[wb.Sheets.Count];

                int i = 0;
                foreach (Microsoft.Office.Interop.Excel.Worksheet item in wb.Sheets)
                {
                    //不包括隐藏表
                    if (item.Visible == Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetVisible)
                    {
                        sheetNames[i++] = item.Name;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("GetExcelSheetNames：读取Excel出现错误\r\n" + ex.Message);
            }
            finally
            {
                //if (wb != null)
                //{
                //    wb.Close(false, objMiss, objMiss);
                //}
                //app.Quit();
                //app = null;

                Kill(app);
            }

            return sheetNames;
        }

        [System.Runtime.InteropServices.DllImport("User32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID);

        public static void Kill(Microsoft.Office.Interop.Excel.Application excel)
        {
            // 得到这个句柄，具体作用是得到这块内存入口
            IntPtr t = new IntPtr(excel.Hwnd);

            int k = 0;
            // 得到本进程唯一标志k
            GetWindowThreadProcessId(t, out k);

            // 得到对进程k的引用
            System.Diagnostics.Process p = System.Diagnostics.Process.GetProcessById(k);

            // 关闭进程k
            p.Kill();
        }

        //-----------------------------------------

        /// <summary>
        /// 获取Access数据
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static DataTable GetAccessTable(string tableName, string fileName)
        {
            return GetOleDbTable(tableName, fileName, GetAccessConnString(fileName));
        }

        /// <summary>
        /// 获取xml数据
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static DataTable GetXmlTable(string fileName)
        {
            DataSet dsXml = new DataSet();
            DataTable dtXml = new DataTable();

            try
            {
                dsXml.ReadXml(fileName);
                dtXml = dsXml.Tables[0];
            }
            catch (Exception ex)
            {
                Console.WriteLine("读取xml出现错误\r\n" + ex.ToString());
            }

            return dtXml;
        }

        /// <summary>
        /// 获取FoxPro数据
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static DataTable GetFoxProTable(string tableName, string fileName)
        {
            return GetOleDbTable(tableName, fileName, GetFoxProConnString(fileName));
        }

        /// <summary>
        /// 获取VisualFoxPro数据
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static DataTable GetVisualFoxProTable(string tableName, string fileName)
        {
            return GetOleDbTable(tableName, fileName, GetVisualFoxProConnString(fileName));
        }

        /// <summary>
        /// 获取Table数据
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fileName"></param>
        /// <param name="connString"></param>
        /// <returns></returns>
        public static DataTable GetOleDbTable(string tableName, string fileName, string connString)
        {
            string sql = string.Empty;
            DataSet ds = null;

            sql = "SELECT * FROM " + tableName;
            ds = GetDataSet(connString, sql, null);

            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }

            return null;
        }

        /// <summary>
        /// 获取Table名字
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="connString"></param>
        /// <returns></returns>
        public static string GetOleDbTableName(string fileName, string connString)
        {
            string tableName = "";

            using (OleDbConnection conn = new OleDbConnection(connString))
            {
                conn.Open();

                DataTable schemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                if (schemaTable != null && schemaTable.Rows.Count > 0)
                {
                    tableName = schemaTable.Rows[schemaTable.Rows.Count - 1]["TABLE_NAME"].ToString();
                }

                //foreach (DataRow dr in schemaTable.Rows)
                //{
                //    //表名   
                //    Console.WriteLine("{0}", dr["TABLE_NAME"]);

                //    //字段名   
                //    DataTable columnTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, new object[] { null, null, dr["TABLE_NAME"].ToString(), null });
                //    foreach (DataRow dr2 in columnTable.Rows)
                //    {
                //        Console.WriteLine("\t {0}", dr2["COLUMN_NAME"]);
                //    }
                //}
            }

            return tableName;
        }

        #region 连接字符串

        public static string GetExcelConnString(string fileName)
        {
            return "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileName + ";Extended Properties='Excel 8.0;IMEX=1'";
        }
        public static string GetAccessConnString(string fileName)
        {
            return "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileName;
        }
        public static string GetFoxProConnString(string fileName)
        {
            return "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileName + ";Extended Properties=dBASE IV;User ID=Admin;Password=";
        }
        public static string GetVisualFoxProConnString(string fileName)
        {
            return "Provider=vfpoledb.1;Data Source=" + fileName + ";Collating Sequence=machine;Mode=Read";
        }

        #endregion

        #region EXCUTE

        private static DataSet GetDataSet(string connString, string sql, params OleDbParameter[] parms)
        {
            OleDbCommand cmd = new OleDbCommand();

            using (OleDbConnection conn = new OleDbConnection(connString))
            {
                PrepareCommand(cmd, conn, sql, parms);

                using (OleDbDataAdapter apt = new OleDbDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    apt.Fill(ds);

                    cmd.Parameters.Clear();

                    return ds;
                }
            }
        }
        private static void PrepareCommand(OleDbCommand cmd, OleDbConnection conn, string cmdText, OleDbParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }

            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            cmd.CommandType = CommandType.Text;

            if (cmdParms != null)
            {
                foreach (OleDbParameter parm in cmdParms)
                {
                    cmd.Parameters.Add(parm);
                }
            }
        }

        #endregion

        #region METHODS

        /// <summary>
        /// 验证数据表列信息
        /// <example>
        /// OleDBUtil.ValidColumn(dt, typeof(string), "数量", "NUM", "QTY");
        /// </example>
        /// </summary>
        /// <param name="dtData">数据表</param>
        /// <param name="type">列数据类型</param>
        /// <param name="columnName">列名</param>
        /// <returns></returns>
        public static string ValidColumn(DataTable dt, Type columnType, params string[] columnName)
        {
            bool hasCol = false;
            string colName = string.Empty;

            for (int i = 0; i < columnName.Length; i++)
            {
                if (dt.Columns.Contains(columnName[i]))
                {
                    if (hasCol)
                    {
                        throw new ApplicationException("数据表中列 " + columnName[i] + " 重复");
                    }
                    else
                    {
                        hasCol = true;
                        colName = columnName[i];
                    }
                }
            }

            if (!hasCol)
            {
                throw new ApplicationException("数据表中不存在列 " + columnName[0]);
            }

            if (dt.Columns[colName].DataType != columnType && columnType != typeof(string))
            {
                throw new ApplicationException("数据表中列 " + colName + " 格式不正确");
            }

            return colName;
        }

        /// <summary>
        /// 获取列名
        /// <example>
        /// OleDBUtil.GetColumnName(dt, "数量", "NUM", "QTY");
        /// </example>
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static string GetColumnName(DataTable dt, params string[] columnName)
        {
            string colName = "";

            for (int i = 0; i < columnName.Length; i++)
            {
                if (dt.Columns.Contains(columnName[i]))
                {
                    colName = columnName[i];
                    break;
                }
            }

            return colName;
        }

        #endregion
    }
}
