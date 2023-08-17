using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

using System.Text.Json.Nodes;

using BaseLib;
using BasicConfig;

namespace Controllers.API
{
    [Tags("Tool")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ToolController : ControllerBase
    {
        /// <summary>
        /// 密碼加密
        /// </summary>
        /// <param name="password">密碼字串</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public ActionResult PaswwordHash(string password)
        {
            AccountHelper.CreatePaswwordHash(password, out byte[] PasswordHash, out byte[] PasswordSalt);

            string HashString = Convert.ToBase64String(PasswordHash);
            string SaltString = Convert.ToBase64String(PasswordSalt);

            JsonObject jo = new()
            {
                { "password", password },
                { "PasswordHash", HashString },
                { "PasswordSalt", SaltString }
            };

            Result result = new()
            {
                ResultCode = ResultCode.Success,
                Data = jo
            };

            return Ok(result);
        }

        /// <summary>
        /// 驗證密碼
        /// </summary>
        /// <param name="password">密碼</param>
        /// <param name="PasswordHash">密碼雜湊</param>
        /// <param name="PasswordSalt">密碼鹽值</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public IActionResult VerifyPassword(string password, string PasswordHash, string PasswordSalt)
        {
            Result result = new();

            try
            {
                bool flag = AccountHelper.VerifyPasswordHash(password, Convert.FromBase64String(PasswordHash), Convert.FromBase64String(PasswordSalt));

                result.ResultCode = flag ? ResultCode.Success : ResultCode.Failed;
                result.Message = flag ? "驗證成功" : "驗證失敗";
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
