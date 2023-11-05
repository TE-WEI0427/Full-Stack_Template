using BaseLib;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SqlCls;
using SqlLib.SqlTool;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Nodes;

namespace Controllers.API
{
    [Tags("AccessToken")]
    [EnableCors("_demoAllowSpecificOrigins")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccessTokenController : ControllerBase
    {
        //readonly string errStr = "";
        readonly StringBuilder strSql = new();

        /// <summary>
        /// Access token 模型
        /// 
        /// 當 Grant_type = refresh_token 時，只需輸入 Refresh_token
        /// </summary>
        public class MDToken
        {
            public string Grant_type { get; set; } = string.Empty; // refresh_token、password
            public string Refresh_token { get; set; } = string.Empty;
            public string Username { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty; // 實際上是要輸入 PasswordHash
        }

        /// <summary>
        /// 外部呼叫 API 的 Token Data
        /// </summary>
        public class MDTokenData
        {
            public string User_UUID { get; set; } = string.Empty;
            public string UseType { get; set; } = string.Empty;
        }

        /// <summary>
        /// 提供目前的存取權 
        /// </summary>
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// 建構元
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public AccessTokenController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 外部呼叫 API 的 使用者 取得 Token
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Token(MDToken model)
        {
            if (model.Grant_type != "refresh_token" && model.Grant_type != "password") return BadRequest("錯誤的 Grant_type");

            var NotFound = Unauthorized("授權失敗");
            var UnAuth = Unauthorized("授權失敗");

            DataTable tbl_QueryTable = new();

            if (model.Grant_type == "password")
            {
                SqlParameter[] param = {
                    new SqlParameter("apiUserId", SqlDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, model.Username),
                    new SqlParameter("PasswordHash", SqlDbType.VarChar, 255, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, model.Password)
                };

                strSql.Clear();
                strSql.AppendLine(" SELECT apiUserPassword, PasswordSalt, user_uuid FROM S10_usersApi WHERE apiUserId=@apiUserId AND PasswordHash=@PasswordHash ");
                tbl_QueryTable = SqlTool.GetDataTable(SqlSetting.StrConnection1, strSql.ToString(), "S10_usersApi", param);

                if (tbl_QueryTable.IsNullOrEmpty()) return NotFound;

                // 驗證密碼是否正確
                byte[] PasswordHash = Convert.FromBase64String(model.Password);
                byte[] PasswordSalt = Convert.FromBase64String(tbl_QueryTable.Rows[0]["PasswordSalt"].ToString() ?? "");
                bool isVerified = AccountHelper.VerifyPasswordHash(tbl_QueryTable.Rows[0]["apiUserPassword"].ToString() ?? "", PasswordHash, PasswordSalt);

                if (isVerified == false || tbl_QueryTable.Rows[0]["user_uuid"].ToString() == "") return UnAuth;
            }

            if (model.Grant_type == "refresh_token")
            {
                if (model.Refresh_token == string.Empty) return UnAuth;

                var jwtSecurityToken = JwtLib.JwtHelper.VerifyToken(model.Refresh_token);
                var jwtData = JwtLib.JwtHelper.GetTokenDataV2<MDTokenData>(jwtSecurityToken ?? new JwtSecurityToken());

                if (jwtData == null || jwtData.UseType != "Refresh") return UnAuth;

                SqlParameter[] param = {
                    new SqlParameter("user_uuid", SqlDbType.UniqueIdentifier, 100, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, new Guid(jwtData.User_UUID))
                };

                strSql.Clear();
                strSql.AppendLine(" SELECT user_uuid, apiUserName FROM S10_usersApi WHERE user_uuid=@user_uuid ");
                tbl_QueryTable = SqlTool.GetDataTable(SqlSetting.StrConnection1, strSql.ToString(), "S10_usersApi", param);

                if (tbl_QueryTable.IsNullOrEmpty()) return UnAuth;
            }

            MDTokenData tokenData = new()
            {
                User_UUID = tbl_QueryTable.Rows[0]["user_uuid"].ToString() ?? "",
                UseType = "Authorize"
            };

            MDTokenData refreshTokenData = new()
            {
                User_UUID = tbl_QueryTable.Rows[0]["user_uuid"].ToString() ?? "",
                UseType = "Refresh"
            };

            int ExpiredDay = 1;

            JsonObject jo = new()
            {
                { "token_type", "Bearer" },
                { "expires_in", 24 * 60 },
                { "access_token", JwtLib.JwtHelper.CreateToken(tokenData, ExpiredDay) },
                { "refresh_token", JwtLib.JwtHelper.CreateToken(refreshTokenData, ExpiredDay) },
                { "issued", ((DateTimeOffset)DateTime.Now).ToUnixTimeMilliseconds() },
                { "expires", ((DateTimeOffset)DateTime.Now.AddDays(ExpiredDay)).ToUnixTimeMilliseconds() }
            };

            return Ok(jo);
        }

        /// <summary>
        /// 取得 User 資料
        /// </summary>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public ActionResult GetUserData()
        {
            var NotFound = Unauthorized("授權失敗");

            var User_UUID = string.Empty;
            var UseType = string.Empty;

            if (_httpContextAccessor.HttpContext != null)
            {
                if (_httpContextAccessor.HttpContext.User.Identity is ClaimsIdentity identity)
                {
                    User_UUID = identity.FindFirst("User_UUID")?.Value ?? "";
                    UseType = identity.FindFirst("UseType")?.Value ?? "";
                }
            }

            if (User_UUID == "" || UseType == "" || UseType != "Authorize") return NotFound;

            SqlParameter[] param = {
                new SqlParameter("user_uuid", SqlDbType.UniqueIdentifier, 100, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, new Guid(User_UUID))
            };

            strSql.Clear();
            strSql.AppendLine(" SELECT apiUserName,email,contactTel FROM S10_usersApi WHERE user_uuid=@user_uuid ");
            DataTable tbl_QueryTable = SqlTool.GetDataTable(SqlSetting.StrConnection1, strSql.ToString(), "S10_usersApi", param);

            if (tbl_QueryTable.IsNullOrEmpty()) return Unauthorized("授權失敗");

            JsonObject jo = new()
            {
                { "User_UUID", User_UUID },
                { "email", tbl_QueryTable.Rows[0]["email"].ToString() ?? "" },
                { "contactTel", tbl_QueryTable.Rows[0]["contactTel"].ToString() ?? "" },
            };

            return Ok(jo);
        }
    }
}
