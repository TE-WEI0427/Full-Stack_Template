using Microsoft.Data.SqlClient;
using System.Data;

using SqlCls;
using SqlLib.SqlTool;

namespace Models.MDAppApi
{
    /// <summary>
    /// App Api Log
    /// </summary>
    public class MDAppApiLog
    {
        /// <summary>
        /// S90_AppApiLog
        /// </summary>
        /// <returns></returns>
        public static DataTable Get_tbl_AppApiLog()
        {
            DataTable tbl_AppApiLog = new("S90_AppApiLog");
            tbl_AppApiLog.Columns.Add("controller", typeof(string));
            tbl_AppApiLog.Columns.Add("action", typeof(string));
            tbl_AppApiLog.Columns.Add("sysUserId", typeof(int));
            tbl_AppApiLog.Columns.Add("UUID", typeof(string));
            tbl_AppApiLog.Columns.Add("actIp", typeof(string));
            tbl_AppApiLog.Columns.Add("webApiLogActType", typeof(string));
            tbl_AppApiLog.Columns.Add("resultCode", typeof(int));
            tbl_AppApiLog.Columns.Add("input", typeof(string));
            tbl_AppApiLog.Columns.Add("message", typeof(string));
            tbl_AppApiLog.Columns.Add("data", typeof(string));
            tbl_AppApiLog.Columns.Add("memo", typeof(string));
            return tbl_AppApiLog;
        }

        /// <summary>
        /// 寫入紀錄
        /// </summary>
        /// <param name="tbl_AppApiLog"></param>
        /// <returns></returns>
        public static string SaveOperLog(DataTable tbl_AppApiLog)
        {
            return SaveOperLog(SqlSetting.StrConnection1, tbl_AppApiLog);
        }

        /// <summary>
        /// 寫入紀錄
        /// </summary>
        /// <param name="strConnection"></param>
        /// <param name="tbl_AppApiLog"></param>
        /// <returns></returns>
        public static string SaveOperLog(string strConnection, DataTable tbl_AppApiLog)
        {
            SqlParameter[] param_Insert = new SqlParameter[11]
            {
                new SqlParameter("controller", SqlDbType.VarChar, 50, "controller"),
                new SqlParameter("action", SqlDbType.VarChar, 50, "action"),
                new SqlParameter("sysUserId", SqlDbType.Int, 4, "sysUserId"),
                new SqlParameter("UUID", SqlDbType.VarChar, 255, "UUID"),
                new SqlParameter("actIp", SqlDbType.VarChar, 50, "actIp"),
                new SqlParameter("webApiLogActType", SqlDbType.Char, 2, "webApiLogActType"),
                new SqlParameter("resultCode", SqlDbType.TinyInt, 4, "resultCode"),
                new SqlParameter("input", SqlDbType.NVarChar, -1, "input"),
                new SqlParameter("message", SqlDbType.NVarChar, -1, "message"),
                new SqlParameter("data", SqlDbType.NVarChar, -1, "data"),
                new SqlParameter("memo", SqlDbType.NVarChar, -1, "memo")
            };
            string strSql_Insert = "insert into S90_AppApiLog (controller,action,sysUserId,UUID,actIp,webApiLogActType,resultCode,input,message,data,memo) values (@controller,@action,@sysUserId,@UUID,@actIp,@webApiLogActType,@resultCode,@input,@message,@data,@memo)";
            return SqlTool.ExecuteBatchActData(strConnection, strSql_Insert, null, null, tbl_AppApiLog, param_Insert, null, null);
        }

        /// <summary>
        /// 寫入紀錄 SqlConnection
        /// </summary>
        /// <param name="cn"></param>
        /// <param name="tmpTbl"></param>
        /// <returns></returns>
        public static string SaveOperLog(SqlConnection cn, DataTable tmpTbl)
        {
            SqlParameter[] param_Insert = new SqlParameter[11]
            {
                new SqlParameter("controller", SqlDbType.VarChar, 50, "controller"),
                new SqlParameter("action", SqlDbType.VarChar, 50, "action"),
                new SqlParameter("sysUserId", SqlDbType.Int, 4, "sysUserId"),
                new SqlParameter("UUID", SqlDbType.VarChar, 255, "UUID"),
                new SqlParameter("actIp", SqlDbType.VarChar, 50, "actIp"),
                new SqlParameter("AppApiLogActType", SqlDbType.Char, 2, "AppApiLogActType"),
                new SqlParameter("resultCode", SqlDbType.TinyInt, 4, "resultCode"),
                new SqlParameter("input", SqlDbType.NVarChar, -1, "input"),
                new SqlParameter("message", SqlDbType.NVarChar, -1, "message"),
                new SqlParameter("data", SqlDbType.NVarChar, -1, "data"),
                new SqlParameter("memo", SqlDbType.NVarChar, -1, "memo")
            };
            string strSql_Insert = "insert into S90_AppApiLog (controller,action,sysUserId,UUID,actIp,webApiLogActType,resultCode,input,message,data,memo) values (@controller,@action,@sysUserId,@UUID,@actIp,@webApiLogActType,@resultCode,@input,@message,@data,@memo)";
            return SqlTool2.ExecuteBatchActData(cn, strSql_Insert, null, null, tmpTbl, param_Insert, null, null);
        }
    }
}
