using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;

using Infrastructure.ApiFilter;
using BasicConfig;
using Service;

namespace Infrastructure
{
    /// <summary>
    /// Api 基底元件
    /// </summary>
    [EnableCors("_demoAllowSpecificOrigins")]
    [Route("api/[controller]/[action]")]
    [ServiceFilter(typeof(ApiActionFilterAttribute))] // (Service-Filter-3)
    [ApiController]
    public class DefaultController : ControllerBase
    {
        /// <summary>
        /// 表示一組索引鍵/值應用程式組態屬性
        /// </summary>
        /// <value></value>
        public IConfiguration? Configuration { get; private set; }
        /// <summary>
        /// 用戶服務
        /// </summary>
        /// <value></value>
        public IUserService? UserService { get; private set; }
        /// <summary>
        /// UserData
        /// </summary>
        /// <value></value>
        public DefalutUserData UserData
        {
            get
            {
                return new()
                {
                    SysUserId = Convert.ToInt32(this.UserService?.GetUserInfo("SysUserId")),
                    Role = this.UserService?.GetUserInfo("Role") ?? "",
                    memo = this.UserService?.GetUserInfo("memo") ?? "",
                };
            }
        }
        #region 建構子
        /// <summary>
        /// Api 基底元件 建構子
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="userService"></param>
        public DefaultController(IConfiguration configuration, IUserService userService)
        {
            this.Configuration = configuration;
            this.UserService = userService;
        }
        #endregion
    }
}
