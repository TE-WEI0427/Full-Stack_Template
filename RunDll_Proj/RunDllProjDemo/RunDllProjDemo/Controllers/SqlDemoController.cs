using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Text;
using System.Text.Json.Nodes;
using Microsoft.Data.SqlClient;

using BasicConfig;
using SqlCls;
using SqlLib.SqlTool;
using BaseLib;

namespace Controllers.API
{
    [Tags("SqlDemo")]
    [EnableCors("_demoAllowSpecificOrigins")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SqlDemoController : ControllerBase
    {
        public class MDSqlDemo
        {
            public class MDSqlInsert
            {
                public string PK_systemId { get; set; } = string.Empty;
                public string PK_systemId2 { get; set; } = string.Empty;
                public string SystemName { get; set; } = string.Empty;
                public string Platform { get; set; } = string.Empty;
                public string VerLeast { get; set; } = string.Empty;
                public string VerLatest { get; set; } = string.Empty;
                public string Url { get; set; } = string.Empty;
                public string Android_url { get; set; } = string.Empty;
                public string IOS_url { get; set; } = string.Empty;
                public string Memo { get; set; } = string.Empty;
            }

            public class MDSqlUpdate
            {
                public string PK_systemId { get; set; } = string.Empty;
                public string Set_version { get; set; } = string.Empty;
            }
        }

        private readonly StringBuilder strSql = new();
        private string errStr = string.Empty;

        /// <summary>
        /// SQL 取資料
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetSqlData()
        {
            Result result = new();

            try
            {
                strSql.Clear();
                strSql.AppendLine(" SELECT * FROM S00_systemConfig ");
                DataTable tbl_QueryTable = SqlTool.GetDataTable(SqlSetting.StrConnection1, strSql.ToString(), "S00_systemConfig");

                JsonObject jo = new()
                {
                    { "tbl_systemConfig", tbl_QueryTable.ConverToJsonArray() },
                };

                result.ResultCode = ResultCode.Success;
                result.Data = jo;
            }
            catch (Exception ex)
            {
                result.ResultCode = ResultCode.Exception;
                result.Message = ex.Message;
            }

            return Ok(result);
        }

        /// <summary>
        /// SQL 帶參數 取資料
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetSqlDataWithParameter(string systemId)
        {
            Result result = new();

            try
            {
                SqlParameter[] param = {
                    new SqlParameter("systemId", SqlDbType.Char, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, systemId),
                };

                string strCond = "WHERE 1=1 AND systemId=@systemId ";

                strSql.Clear();
                strSql.AppendLine(" SELECT * FROM S00_systemConfig " + strCond);
                DataTable tbl_QueryTable = SqlTool.GetDataTable(SqlSetting.StrConnection1, strSql.ToString(), "S00_systemConfig", param);

                if (tbl_QueryTable.IsNullOrEmpty())
                {
                    result.ResultCode = ResultCode.NotFound;
                    result.Message = "查無資料";
                }
                else
                {
                    JsonObject jo = new()
                    {
                        { "tbl_systemConfig", tbl_QueryTable.ConverToJsonArray() },
                    };

                    result.ResultCode = ResultCode.Success;
                    result.Data = jo;
                }                
            }
            catch (Exception ex)
            {
                result.ResultCode = ResultCode.Exception;
                result.Message = ex.Message;
            }

            return Ok(result);
        }

        /// <summary>
        /// SQL 新增資料
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SqlInsert(MDSqlDemo.MDSqlInsert model)
        {
            Result result = new();

            try
            {
                SqlParameter[] param = {
                    new SqlParameter("systemId", SqlDbType.Char, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, model.PK_systemId),
                    new SqlParameter("systemId2", SqlDbType.Char, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, model.PK_systemId2),
                    new SqlParameter("systemName", SqlDbType.NVarChar, 255, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, model.SystemName),
                    new SqlParameter("platform", SqlDbType.NVarChar, 255, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, model.Platform),
                    new SqlParameter("verLeast", SqlDbType.NVarChar, 255, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, model.VerLeast),
                    new SqlParameter("verLatest", SqlDbType.NVarChar, 255, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, model.VerLatest),
                    new SqlParameter("url", SqlDbType.NVarChar, 255, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, model.Url),
                    new SqlParameter("android_url", SqlDbType.NVarChar, int.MaxValue, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, model.Android_url),
                    new SqlParameter("iOS_url", SqlDbType.NVarChar, int.MaxValue, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, model.IOS_url),
                    new SqlParameter("memo", SqlDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, model.Memo),
                };


                strSql.Clear();
                strSql.AppendLine(" INSERT INTO S00_systemConfig ");
                strSql.AppendLine(" (systemId, systemName, platform, verLeast, verLatest, url, android_url, iOS_url, memo) ");
                strSql.AppendLine(" VALUES ");
                strSql.AppendLine(" (@systemId, @systemName, @platform, @verLeast, @verLatest, @url, @android_url, @iOS_url, @memo) ");
                errStr = SqlTool.ExecuteNonQuery(SqlSetting.StrConnection1, strSql.ToString(), param);

                strSql.Clear();
                strSql.AppendLine(" INSERT INTO S00_systemConfig ");
                strSql.AppendLine(" (systemId, systemName, platform, verLeast, verLatest, url, android_url, iOS_url, memo) ");
                strSql.AppendLine(" VALUES ");
                strSql.AppendLine(" (@systemId2, @systemName, @platform, @verLeast, @verLatest, @url, @android_url, @iOS_url, @memo) ");
                int count = SqlTool.ExecuteNonQueryNum(SqlSetting.StrConnection1, strSql.ToString(), param);

                if (!string.IsNullOrEmpty(errStr))
                {
                    result.ResultCode = ResultCode.Failed;
                    result.Message = errStr;
                }
                else if (count == 0) // 異動筆數
                {
                    result.ResultCode = ResultCode.NotFound;
                    result.Message = "無新增任何資料";
                }
                else
                {
                    result.ResultCode = ResultCode.Success;
                    result.Message = "新增成功";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = ResultCode.Exception;
                result.Message = ex.Message;
            }

            return Ok(result);
        }

        /// <summary>
        /// SQL 更新資料
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public IActionResult SqlUpdate(MDSqlDemo.MDSqlUpdate model)
        {
            Result result = new();

            try
            {
                SqlParameter[] param = {
                    new SqlParameter("systemId", SqlDbType.Char, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, model.PK_systemId),
                    new SqlParameter("version", SqlDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, model.Set_version),
                };

                string strCond = "WHERE 1=1 AND systemId=@systemId ";

                strSql.Clear();
                strSql.AppendLine(" UPDATE S00_systemConfig SET version=@version " + strCond);
                errStr = SqlTool.ExecuteNonQuery(SqlSetting.StrConnection1, strSql.ToString(), param);

                strSql.Clear();
                strSql.AppendLine(" UPDATE S00_systemConfig SET version=@version " + strCond);
                int count = SqlTool.ExecuteNonQueryNum(SqlSetting.StrConnection1, strSql.ToString(), param);

                if (!string.IsNullOrEmpty(errStr))
                {
                    result.ResultCode = ResultCode.Failed;
                    result.Message = errStr;
                }
                else if (count == 0) // 異動筆數
                {
                    result.ResultCode = ResultCode.NotFound;
                    result.Message = "無更新任何資料";
                }
                else
                {
                    result.ResultCode = ResultCode.Success;
                    result.Message = "更新成功";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = ResultCode.Exception;
                result.Message = ex.Message;
            }

            return Ok(result);
        }

        /// <summary>
        /// SQL 刪除資料
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        public IActionResult SqlDelete(string systemId)
        {
            Result result = new();

            try
            {
                SqlParameter[] param = {
                    new SqlParameter("systemId", SqlDbType.Char, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, systemId),
                };

                string strCond = "WHERE 1=1 AND systemId=@systemId ";

                strSql.Clear();
                strSql.AppendLine(" DELETE S00_systemConfig " + strCond);
                errStr = SqlTool.ExecuteNonQuery(SqlSetting.StrConnection1, strSql.ToString(), param);

                if (!string.IsNullOrEmpty(errStr))
                {
                    result.ResultCode = ResultCode.Failed;
                    result.Message = errStr;
                }
                else
                {
                    result.ResultCode = ResultCode.Success;
                    result.Message = "刪除成功";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = ResultCode.Exception;
                result.Message = ex.Message;
            }

            return Ok(result);
        }
    }
}
