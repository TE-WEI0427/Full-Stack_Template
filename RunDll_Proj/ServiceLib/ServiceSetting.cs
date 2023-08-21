using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using Service;

namespace ServiceLib
{
    public static class ServiceSetting
    {
        /// <summary>
        /// 依賴注入
        /// </summary>
        /// <param name="builder">指定服務描述項集合的合約</param>
        public static void AddServiceScoped(this WebApplicationBuilder builder)
        {
            // UserService
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddHttpContextAccessor();
        }
    }
}
