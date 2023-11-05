using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json.Nodes;
using System.Data;
using Microsoft.Data.SqlClient;

using Infrastructure;
using Service;
using Models.MDAppApi;

using BasicConfig;
using BaseLib;
using SqlCls;
using SqlLib.SqlTool;
using MailLib;
using Microsoft.AspNetCore.Identity;
using System.Reflection;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using static System.Net.WebRequestMethods;
using Microsoft.IdentityModel.Tokens;

namespace Controllers.App
{
    [Tags("AppSystem")]
    public class AppSystemController : DefaultController
    {
        private readonly StringBuilder strSql = new();
        private string errStr = string.Empty;

        public AppSystemController(IConfiguration configuration, IUserService userService, ILogService logService) : base(configuration, userService, logService) { }

        #region Private Method

        /// <summary>
        /// 註冊帳號 寄送電子郵件訊息
        /// </summary>
        /// <param name="Email">電子郵件</param>
        /// <param name="url">導向網址</param>
        /// <returns></returns>
        private static string RegisterToSendEmail(string Email, string url)
        {
            // send email
            MailDTO mailDTO = new()
            {
                To = Email,
                Subject = "註冊帳戶",
                Body = "" +
                "＊＊＊＊＊＊＊＊　注意　＊＊＊＊＊＊＊＊<br>" +
                "<span style='margin-left:50px;color:#EA0000;'>此信為系統發出，請勿回覆</span><br>" +
                "＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊<br><br>" +
                "<span>您好，感謝您申請註冊</span><br><br>" +
                "<span>請點擊下方網址啟動帳號。</span><br><br>" +
                "<a href='" + url + "'>啟動帳戶</a>"
            };

            return MailHelper.SendMail.Send(mailDTO);
        }

        #endregion

        /// <summary>
        /// 註冊
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Todo
        ///     {
        ///        "userId": "Andy",
        ///        "password": "19980427",
        ///        "email": "vm62k6jo6@gmai.com"
        ///     }
        /// </remarks>
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Register(MDSys.Register model)
        {
            Result result = new();

            try
            {
                /* 流程說明
                 * 1. 先確認帳號、密碼與電子郵件是否已進行過註冊
                 * 2. 若已註冊，發送帳號啟動郵件
                 * 3. 若未註冊走正常建立資料流程
                 */

                //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

                // 確認帳號、密碼與電子郵件是否已進行過註冊
                SqlParameter[] param = {
                    new SqlParameter("userId", SqlDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, model.UserId),
                    new SqlParameter("userPassword", SqlDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, model.Password),
                    new SqlParameter("userEmail", SqlDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, model.Email)
                };
                strSql.Clear();
                strSql.AppendLine(" SELECT sysUserId FROM S10_users WHERE userId=@userId AND userPassword=@userPassword AND userEmail=@userEmail ");
                DataTable tbl_users = SqlTool.GetDataTable(SqlSetting.StrConnection1, strSql.ToString(), "S10_users", param);

                if (tbl_users.IsNullOrEmpty() == false)
                {// 已註冊

                    string? sysUserId = tbl_users.Rows[0]["sysUserId"].ToString();

                    // 重新雜湊密碼
                    AccountHelper.CreatePaswwordHash(model.Password, out byte[] PasswordHash, out byte[] PasswordSalt);

                    // 更新密碼雜湊值
                    SqlParameter[] param2 = {
                        new SqlParameter("sysUserId", SqlDbType.Int, 4, ParameterDirection.InputOutput, false, 0, 0, "", DataRowVersion.Proposed, sysUserId),
                        new SqlParameter("userId", SqlDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, model.UserId),
                        new SqlParameter("userPassword", SqlDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, model.Password),
                        new SqlParameter("PasswordHash", SqlDbType.VarChar, 255, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, Convert.ToBase64String(PasswordHash)),
                        new SqlParameter("PasswordSalt", SqlDbType.VarChar, 255, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, Convert.ToBase64String(PasswordSalt)),
                        new SqlParameter("userEmail", SqlDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, model.Email)
                    };
                    strSql.Clear();
                    strSql.AppendLine(" UPDATE S10_users SET PasswordHash=@PasswordHash, PasswordSalt=@PasswordSalt WHERE userId=@userId AND userPassword=@userPassword AND userEmail=@userEmail ");
                    SqlTool.ExecuteNonQuery(SqlSetting.StrConnection1, strSql.ToString(), param2);

                    // 發送郵件
                    string key = "StartUpKey";
                    string val = string.Format("{0}:{1}", tbl_users.Rows[0]["sysUserId"].ToString(), Convert.ToBase64String(PasswordHash));
                    string url = "https://localhost:7086/html/UserStartUp.html?" + key.Base64UrlEncode() + "=" + val.Base64UrlEncode();
                    errStr = RegisterToSendEmail(model.Email, url);

                    // 發送郵件失敗 拋出例外錯誤
                    if (string.IsNullOrEmpty(errStr) == false) throw new Exception(errStr);
                }
                else
                {// 未註冊

                    // 雜湊密碼
                    AccountHelper.CreatePaswwordHash(model.Password, out byte[] PasswordHash, out byte[] PasswordSalt);

                    // 建立臨時帳戶名稱
                    string userName = "@" + model.UserId + CommonUtil.GenRandomStr(5);

                    int sysUserId = 0;

                    // 建立資料
                    SqlParameter[] param2 = {
                        new SqlParameter("sysUserId", SqlDbType.Int, 4, ParameterDirection.InputOutput, false, 0, 0, "", DataRowVersion.Proposed, sysUserId),
                        new SqlParameter("userId", SqlDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, model.UserId),
                        new SqlParameter("userName", SqlDbType.NVarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, userName),
                        new SqlParameter("userPassword", SqlDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, model.Password),
                        new SqlParameter("PasswordHash", SqlDbType.VarChar, 255, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, Convert.ToBase64String(PasswordHash)),
                        new SqlParameter("PasswordSalt", SqlDbType.VarChar, 255, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, Convert.ToBase64String(PasswordSalt)),
                        new SqlParameter("userEmail", SqlDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, model.Email)
                    };
                    strSql.Clear();
                    strSql.AppendLine(" INSERT INTO S10_users  ");
                    strSql.AppendLine(" (userId, userName, userPassword, PasswordHash, PasswordSalt, userEmail) ");
                    strSql.AppendLine(" VALUES ");
                    strSql.AppendLine(" (@userId, @userName, @userPassword, @PasswordHash, @PasswordSalt, @userEmail) ");
                    strSql.AppendLine(" SELECT @sysUserId = SCOPE_IDENTITY() ");
                    errStr = SqlTool.ExecuteNonQuery(SqlSetting.StrConnection1, strSql.ToString(), param);

                    if (string.IsNullOrEmpty(errStr))
                    {
                        // 發送郵件
                        string key = "StartUpKey";
                        string val = string.Format("{0}:{1}", sysUserId, Convert.ToBase64String(PasswordHash));
                        string url = "https://localhost:7086/html/UserStartUp.html?" + key.Base64UrlEncode() + "=" + val.Base64UrlEncode();
                        errStr = RegisterToSendEmail(model.Email, url);

                        // 發送郵件失敗 拋出例外錯誤
                        if (string.IsNullOrEmpty(errStr) == false) throw new Exception(errStr);
                    }
                    else
                    {
                        // 建立帳戶資料失敗，若是唯一索引的錯誤，覆寫錯誤訊息
                        if (errStr.Contains("唯一索引")) errStr = "註冊失敗，該帳號已被註冊";
                    }
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = ResultCode.Exception;
                result.Message = ex.Message;
            }

            // 未執行例外錯誤
            if (result.ResultCode != ResultCode.Exception)
            {
                // 確認整體流程成功還是失敗
                if (string.IsNullOrEmpty(errStr) == false)
                {
                    result.ResultCode = ResultCode.Failed;
                    result.Message = errStr;
                }
                else
                {
                    result.ResultCode = ResultCode.Success;
                    result.Message = "註冊完成，請至信箱進行確認";
                }
            }

            return Ok(result);
        }

        /// <summary>
        /// 啟動帳號
        /// </summary>
        /// <param name="StartUpKey"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public ActionResult UserStartUp(string StartUpKey)
        {
            Result result = new();

            try
            {
                // Base64Url 解碼
                string decodedString = StartUpKey.Base64UrlDecode();
                // 切刻字串 取得 sysUserId & PasswordHash
                List<string> list = decodedString.Split(":").ToList();

                // 取出 使用者的密碼與密碼鹽值
                SqlParameter[] param = {
                    new SqlParameter("sysUserId", SqlDbType.Int, 4, ParameterDirection.InputOutput, false, 0, 0, "", DataRowVersion.Proposed, int.Parse(list[0])),
                };
                strSql.Clear();
                strSql.AppendLine(" SELECT userPassword, PasswordSalt FROM S10_users WHERE sysUserId=@sysUserId ");
                DataTable tbl_QueryTable = SqlTool.GetDataTable(SqlSetting.StrConnection1, strSql.ToString(), "S10_users", param);

                if (tbl_QueryTable.IsNullOrEmpty() == false)
                {
                    // 進行密碼驗證
                    string Password = tbl_QueryTable.Rows[0]["userPassword"].ToString() ?? "";
                    string PasswordHash = list[1];
                    string PasswordSalt = tbl_QueryTable.Rows[0]["PasswordSalt"].ToString() ?? "";
                    bool isVerify = AccountHelper.VerifyPasswordHash(Password, Convert.FromBase64String(PasswordHash), Convert.FromBase64String(PasswordSalt));

                    if (isVerify == false) throw new Exception("資料異常，請重新註冊帳戶");
                }
                else
                {
                    errStr = "該電子郵件訊息已失效，請重新註冊帳戶。";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = ResultCode.Failed;
                result.Message = ex.Message;
            }

            // 未執行例外錯誤
            if (result.ResultCode != ResultCode.Exception)
            {
                // 確認整體流程成功還是失敗
                if (string.IsNullOrEmpty(errStr) == false)
                {
                    result.ResultCode = ResultCode.Failed;
                    result.Message = errStr;
                }
                else
                {
                    result.ResultCode = ResultCode.Success;
                    result.Message = "註冊完成，請至信箱進行確認";
                }
            }

            return Ok(result);
        }

        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(AppUserData model)
        {
            Result result = new();

            JsonObject jo = new()
            {
                { "token", JwtLib.JwtHelper.CreateToken(model, 365) }
            };

            result.Data = jo;

            return Ok(result);
        }

        /// <summary>
        /// 權限驗證
        /// </summary>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public ActionResult CheckAuth()
        {
            Result result = new();

            JsonObject jo = new()
            {
                { "SysUserId", UserData.SysUserId },
                { "Platform", UserData.Platform },
                { "UUID", UserData.UUID },
                { "Memo", UserData.Memo },
                { "ClientIp", UserData.ClientIp }
            };

            result.Data = jo;

            return Ok(result);
        }
    }
}
