using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Text;
using System.Text.Json.Nodes;

using BasicConfig;
using Infrastructure;
using Service;

namespace Controllers.API
{
    [Tags("Test")]
    public class TestController : DefaultController
    {
        private readonly StringBuilder strSql = new();
        private string errStr = string.Empty;

        public TestController(IConfiguration configuration, IUserService userService, ILogService logService) : base(configuration, userService, logService)
        {
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
                { "center_token", JwtLib.JwtHelper.CreateToken(model, 365) }
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
                { "Model", UserData.Model },
                { "UUID", UserData.UUID },
                { "Memo", UserData.Memo },
                { "ClientIp", UserData.ClientIp }
            };

            result.Data = jo;

            return Ok(result);
        }
    }
}
