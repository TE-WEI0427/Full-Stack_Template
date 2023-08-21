using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Controllers.API
{
    public class MDJwtDemo
    {
        public class MDKeyVal
        {
            public string Key1 { get; set; } = string.Empty;
            public string Key2 { get; set; } = string.Empty;
        }

        public class MDUserData
        {
            public string UserName { get; set; } = string.Empty;
            public string UserRole { get; set; } = string.Empty;
        }
    }

    [Tags("JwtDemo")]
    [EnableCors("_demoAllowSpecificOrigins")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class JwtDemoController : ControllerBase
    {
        /// <summary>
        /// 提供目前的存取權 
        /// </summary>
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// 建構元
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public JwtDemoController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 取得 Token
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public ActionResult GetToken(MDJwtDemo.MDUserData model)
        {
            string token = JwtLib.JwtHelper.CreateToken(model, 1);

            return Ok(token);
        }

        /// <summary>
        /// 無須授權的 Get
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetWithNoAuth()
        {
            return Ok("Good Job!");
        }

        /// <summary>
        /// 無須授權的 Post
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public ActionResult PostWithNoAuth(MDJwtDemo.MDKeyVal model)
        {
            return Ok(model);
        }

        /// <summary>
        /// 從 token 取得資料
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public ActionResult GetValueFromToken(string Key)
        {
            var value = string.Empty;

            if (_httpContextAccessor.HttpContext != null)
            {
                if (_httpContextAccessor.HttpContext.User.Identity is ClaimsIdentity identity)
                {
                    value = identity.FindFirst(Key)?.Value ?? "";
                }
            }

            return Ok(value);
        }

        /// <summary>
        /// 從 token 取得完整資料
        /// </summary>
        /// <param name="Token"></param>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public ActionResult GetModelFromToken(string Token)
        {
            var jwtSecurityToken = JwtLib.JwtHelper.VerifyToken(Token);
            var model = JwtLib.JwtHelper.GetTokenDataV2<MDJwtDemo.MDUserData>(jwtSecurityToken ?? new JwtSecurityToken());

            return Ok(model);
        }

        /// <summary>
        /// 須授權的 Get
        /// </summary>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public ActionResult GetWithAuth()
        {
            return Ok("[Auth] Good Job!");
        }

        /// <summary>
        /// 須授權的 Post
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public ActionResult PostWithAuth(MDJwtDemo.MDKeyVal model)
        {
            return Ok(model);
        }
    }
}
