using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Service
{
    /// <summary>
    /// 用戶驗證後要做的事 實作功能介面
    /// </summary>
    public class UserService : IUserService
    {
        /// <summary>
        /// 提供目前的存取權 
        /// </summary>
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// 建構元
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 從 HttpContext 中找到 Key 的 Value
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public string GetUserInfo(string Key)
        {
            var result = string.Empty;

            if (_httpContextAccessor.HttpContext != null) 
            {
                if (_httpContextAccessor.HttpContext.User.Identity is ClaimsIdentity identity)
                {
                    result = identity.FindFirst(Key)?.Value ?? "";
                }
            }

            return result;
        }
    }
}
