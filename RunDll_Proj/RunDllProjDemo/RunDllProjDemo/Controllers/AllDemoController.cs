using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

using System.Text.Json.Nodes;

using Infrastructure;
using BasicConfig;
using BaseLib;
using JwtLib;
using Service;
using Infrastructure.ApiFilter;

namespace Controllers.API
{
    [Tags("AllDemo")]
    [EnableCors("_demoAllowSpecificOrigins")]
    [Route("api/[controller]/[action]")]
    [ServiceFilter(typeof(ApiActionFilterAttribute))] // (Service-Filter-3)
    [ApiController]
    public class AllDemoController : DefaultController
    {
        public class MDAllDemo
        {
            public class MDUserData
            {
                public int SysUserId { get; set; } = 0;
                public string Name { get; set; } = string.Empty;
                public string Role { get; set; } = string.Empty;
                public string Title { get; set; } = string.Empty;
                public int Gender { get; set; }
                public int Age { get; set; }

                public string Account { get; set; } = string.Empty;
                public string Password { get; set; } = string.Empty;
                public string PasswordHash { get; set; } = string.Empty;
                public string PasswordSalt { get; set; } = string.Empty;
            }

            public class MDLogin
            {
                public string Account { get; set; } = string.Empty;
                public string Password { get; set; } = string.Empty;
            }
        }

        string errStr = "";

        private readonly List<MDAllDemo.MDUserData> UserList = new();

        /// <summary>
        /// 建構元
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="userService"></param>
        public AllDemoController(IConfiguration configuration, IUserService userService) : base(configuration, userService)
        {
            UserList.Add(new MDAllDemo.MDUserData
            { 
                SysUserId = 1, Name = "User 1", Role = "admin", Title = "Manager", Gender = 1, Age = 35, 
                Account = "User1", Password = "admin", 
                PasswordHash = "mETwOisMQDrGxnJtlw9sOXq5UtB0rWVgUZ4mVkSp4479gXYVhg4GLVC8rZHVCNnv4LeUWWxyPQIZxaY8n1GSjA==", 
                PasswordSalt = "Gp73DSFK85I5lQf7gaTG7QMMazE1hhXyam9k5OsI1p6etbvlHkBdO1sUvsvUfDf/d11LvXLPcvySvZwNXBCAOSBxww5d26yawGcp+DJEN9yDidGmkLhoYIB6dQpz+TZ2HBedgVBLdTPsxvi2LuOgcjs2kFLp+i2Oa/4wtspI2OQ="
            });
            UserList.Add(new MDAllDemo.MDUserData 
            { 
                SysUserId = 2, Name = "User 2", Role = "emp1", Title = "engineer", Gender = 0, Age = 27, 
                Account = "User2", Password = "emp1", 
                PasswordHash = "U0TsIfj/H9JTjzYiw9NcINHHUHWx4nbt0B+4R5xicTNCom0+xFFgTJUV1GfkqBvSfcEYrhqbkmiqQFcydCN2yg==", 
                PasswordSalt = "AHKfFXaYU03SMqKWbo+2sFspv9+c5M5D3ZbglacgMXhB8tVI1u5FERqqk7agBRar2vZcBsdp15WHGA22bKSpdlfoKAjVCT+Zkx7O4OGBUs7JOwmcmMnWyvxxaqXs0UaZdwU/Gq+AvXygf1EVA+uMFHZw5DPXghLEsPqFGnWqaCU="
            });
            UserList.Add(new MDAllDemo.MDUserData 
            { 
                SysUserId = 3, Name = "User 3", Role = "emp2", Title = "assistant engineer", Gender = 1, Age = 25, 
                Account = "User3", Password = "emp2", 
                PasswordHash = "RYscm/XNCFWjHRERbefK6ip5uPbJzCnA57ukQ6REYbiM5DKwO/itRz/xYboRmETHmVZsidf27/nZLrrdH41cnw==", 
                PasswordSalt = "TMpZLVghjZYOlPURk/XsUDZMCKhqzibCu+WoB5UFylvGPpPgT58S0z8sd2ptHLRDMM0iGH+2BkeAylE418HPFJJjlY3FRZSsw8wFAy0cJCzFbBxvgMn37FhY5tF7uuR6qHFK32bGpk9MwC0GnhDZ/5zMCaVIiLdPoOR80tjnvbw="
            });
        }

        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(MDAllDemo.MDLogin model)
        {
            Result result = new();
            JsonObject jo = new();

            // 取得 User 資料
            var user = UserList.Find(x => x.Account == model.Account);

            // 有 User 資料
            if (user != null)
            {
                // 驗證密碼是否正確
                byte[] PasswordHash = Convert.FromBase64String(user.PasswordHash);
                byte[] PasswordSalt = Convert.FromBase64String(user.PasswordSalt);
                bool isVerified = AccountHelper.VerifyPasswordHash(model.Password, PasswordHash, PasswordSalt);

                if (isVerified)
                {
                    UserData userData = new()
                    {
                        SysUserId = user.SysUserId,
                        Role = user.Role
                    };

                    string token = JwtHelper.CreateToken(userData, 1);

                    jo.Add("jwtToken", token);
                }
                else
                {
                    errStr = "密碼錯誤";
                }
            }
            else
            {
                errStr = "查無帳戶";
            }

            if (string.IsNullOrEmpty(errStr))
            {
                result.ResultCode = ResultCode.Success;
                result.Data = jo;
            }
            else
            {
                result.ResultCode = ResultCode.Failed;
                result.Message = errStr;
            }

            return Ok(result);
        }

        /// <summary>
        /// 權限驗證
        /// </summary>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public IActionResult CheckAuth()
        {
            Result result = new();

            try
            {
                JsonObject jo = new()
                {
                    { "SysUserId", UserData.SysUserId },
                    { "Role", UserData.Role },
                    { "memo", UserData.memo }
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
        /// API 內執行權限驗證 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult CheckAuthV2(string jwtToken)
        {
            Result result = new();

            try
            {
                var jwtSecurityToken = JwtHelper.VerifyToken(jwtToken);

                UserData userData = JwtHelper.GetTokenDataV2<UserData>(jwtSecurityToken ?? new JwtSecurityToken()) ?? new();

                result.ResultCode = ResultCode.Success;
                result.Data = userData;
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
