using Microsoft.AspNetCore.Mvc.Infrastructure;
using Newtonsoft.Json;
using System.Data;
using System.Text.Encodings.Web;
using System.Text.Json;

using BasicConfig;
using Models.MDAppApi;

namespace Service
{
    /// <summary>
    /// 寫入紀錄 實作功能介面
    /// </summary>
    public class LogService : ILogService
    {
        /// <summary>
        /// 提供目前的存取權 
        /// </summary>
        private readonly IActionContextAccessor _actionContextAccessor;

        /// <summary>
        /// 建構元
        /// </summary>
        /// <param name="actionContextAccessor"></param>
        public LogService(IActionContextAccessor actionContextAccessor)
        {
            _actionContextAccessor = actionContextAccessor;
        }

        /// <summary>
        /// 開始呼叫紀錄 POST
        /// </summary>
        /// <param name="input">輸入參數</param>
        /// <param name="userData">用戶端資料</param>
        /// <param name="webApiLogAct">動作類型 開始/結束</param>
        /// <returns></returns>
        public string AppStartLog<T>(T input, AppUserData userData, WebApiLogActType webApiLogAct)
        {
            DataTable tbl_webApiLog = MDAppApiLog.Get_tbl_AppApiLog();
            DataRow operDr = tbl_webApiLog.NewRow();
            if (_actionContextAccessor.ActionContext != null)
            {
                operDr["controller"] = _actionContextAccessor.ActionContext.RouteData.Values["controller"]?.ToString() ?? "";
                operDr["action"] = _actionContextAccessor.ActionContext.RouteData.Values["action"]?.ToString() ?? ""; ;
                operDr["sysUserId"] = userData.SysUserId == 0 ? DBNull.Value : userData.SysUserId;
                operDr["UUID"] = userData.UUID;
                operDr["actIp"] = userData.ClientIp;
                operDr["webApiLogActType"] = (int)webApiLogAct;
                operDr["resultCode"] = DBNull.Value;
                operDr["input"] = JsonConvert.SerializeObject(input);
                operDr["message"] = DBNull.Value;
                operDr["data"] = DBNull.Value;
                operDr["memo"] = userData.Memo == "" ? DBNull.Value : userData.Memo;

                tbl_webApiLog.Rows.Add(operDr);
            }
            return MDAppApiLog.SaveOperLog(tbl_webApiLog);
        }

        /// <summary>
        /// 開始呼叫紀錄 GET
        /// </summary>
        /// <param name="userData">用戶端資料</param>
        /// <param name="webApiLogAct">動作類型 開始/結束</param>
        /// <returns></returns>
        public string AppStartLog(AppUserData userData, WebApiLogActType webApiLogAct)
        {
            DataTable tbl_webApiLog = MDAppApiLog.Get_tbl_AppApiLog();
            DataRow operDr = tbl_webApiLog.NewRow();
            if (_actionContextAccessor.ActionContext != null)
            {
                operDr["controller"] = _actionContextAccessor.ActionContext.RouteData.Values["controller"]?.ToString() ?? "";
                operDr["action"] = _actionContextAccessor.ActionContext.RouteData.Values["action"]?.ToString() ?? ""; ;
                operDr["sysUserId"] = userData.SysUserId == 0 ? DBNull.Value : userData.SysUserId;
                operDr["UUID"] = userData.UUID;
                operDr["actIp"] = userData.ClientIp;
                operDr["webApiLogActType"] = (int)webApiLogAct;
                operDr["resultCode"] = DBNull.Value;
                operDr["input"] = DBNull.Value;
                operDr["message"] = DBNull.Value;
                operDr["data"] = DBNull.Value;
                operDr["memo"] = userData.Memo == "" ? DBNull.Value : userData.Memo;

                tbl_webApiLog.Rows.Add(operDr);
            }
            return MDAppApiLog.SaveOperLog(tbl_webApiLog);
        }

        /// <summary>
        /// 結束呼叫紀錄 POST
        /// </summary>
        /// <param name="input">輸入參數</param>
        /// <param name="userData">用戶端資料</param>
        /// <param name="result">輸出資料</param>
        /// <param name="webApiLogAct">動作類型 開始/結束</param>
        /// <param name="isBigData">是否為大型資料</param>
        /// <returns></returns>
        public string AppEndLog<T>(T input, AppUserData userData, Result result, WebApiLogActType webApiLogAct, bool isBigData = false)
        {
            DataTable tbl_webApiLog = MDAppApiLog.Get_tbl_AppApiLog();
            DataRow operDr = tbl_webApiLog.NewRow();
            if (_actionContextAccessor.ActionContext != null)
            {
                operDr["controller"] = _actionContextAccessor.ActionContext.RouteData.Values["controller"]?.ToString() ?? "";
                operDr["action"] = _actionContextAccessor.ActionContext.RouteData.Values["action"]?.ToString() ?? ""; ;
                operDr["sysUserId"] = userData.SysUserId == 0 ? DBNull.Value : userData.SysUserId;
                operDr["UUID"] = userData.UUID;
                operDr["actIp"] = userData.ClientIp;
                operDr["webApiLogActType"] = (int)webApiLogAct;
                operDr["resultCode"] = result.ResultCode;
                operDr["input"] = JsonConvert.SerializeObject(input);
                operDr["message"] = result.Message == "" ? DBNull.Value : result.Message;
                operDr["data"] = isBigData ? DBNull.Value : (result.Data.GetType().Name != "JsonObject" ? JsonConvert.SerializeObject(result.Data) : System.Text.Json.JsonSerializer.Serialize(result.Data, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping }));
                operDr["memo"] = userData.Memo == "" ? DBNull.Value : userData.Memo;

                tbl_webApiLog.Rows.Add(operDr);
            }
            return MDAppApiLog.SaveOperLog(tbl_webApiLog);
        }

        /// <summary>
        /// 結束呼叫紀錄 GET
        /// </summary>
        /// <param name="userData">用戶端資料</param>
        /// <param name="result">輸出資料</param>
        /// <param name="webApiLogAct">動作類型 開始/結束</param>
        /// <param name="isBigData">是否為大型資料</param>
        /// <returns></returns>
        public string AppEndLog(AppUserData userData, Result result, WebApiLogActType webApiLogAct, bool isBigData = false)
        {
            DataTable tbl_webApiLog = MDAppApiLog.Get_tbl_AppApiLog();
            DataRow operDr = tbl_webApiLog.NewRow();
            if (_actionContextAccessor.ActionContext != null)
            {
                operDr["controller"] = _actionContextAccessor.ActionContext.RouteData.Values["controller"]?.ToString() ?? "";
                operDr["action"] = _actionContextAccessor.ActionContext.RouteData.Values["action"]?.ToString() ?? ""; ;
                operDr["sysUserId"] = userData.SysUserId == 0 ? DBNull.Value : userData.SysUserId;
                operDr["UUID"] = userData.UUID;
                operDr["actIp"] = userData.ClientIp;
                operDr["webApiLogActType"] = (int)webApiLogAct;
                operDr["resultCode"] = result.ResultCode;
                operDr["input"] = DBNull.Value;
                operDr["message"] = result.Message == "" ? DBNull.Value : result.Message;
                operDr["data"] = isBigData ? DBNull.Value : (result.Data.GetType().Name != "JsonObject" ? JsonConvert.SerializeObject(result.Data) : System.Text.Json.JsonSerializer.Serialize(result.Data, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping }));
                operDr["memo"] = userData.Memo == "" ? DBNull.Value : userData.Memo;

                tbl_webApiLog.Rows.Add(operDr);
            }
            return MDAppApiLog.SaveOperLog(tbl_webApiLog);
        }
    }
}
