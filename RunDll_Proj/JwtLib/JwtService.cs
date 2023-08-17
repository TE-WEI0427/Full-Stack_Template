using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

using System.Text;

namespace JwtLib
{
    /// <summary>
    /// JWT 服務
    /// </summary>
    public static class JwtService
    {
        /// <summary>
        /// 加入 JWT 驗證服務
        /// </summary>
        /// <param name="builder">指定服務描述項集合的合約</param>
        /// <param name="clockSkew">過期時間容錯值</param>
        public static void AddJwtAuthentication(this WebApplicationBuilder builder, TimeSpan clockSkew)
        {
            builder.Services
                .AddAuthentication()
                .AddJwtBearer("Bearer", options =>
                {
                    // 當驗證失敗時，回應標頭會包含 WWW-Authenticate 標頭，這裡會顯示失敗的詳細錯誤原因
                    options.IncludeErrorDetails = true;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = (!string.IsNullOrEmpty(JwtSettings.SecretKey)),//是否驗證Issuer
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSettings.SecretKey)), //SecurityKey
                        ValidateIssuer = (!string.IsNullOrEmpty(JwtSettings.Issuer)),//是否驗證Issuer
                        ValidIssuer = JwtSettings.Issuer, //發行人Issuer
                        ValidateAudience = (!string.IsNullOrEmpty(JwtSettings.Audience)), //是否驗證Audience
                        ValidAudience = JwtSettings.Audience, //訂閱人Audience
                        ValidateLifetime = true,//是否驗證失效時間
                        ClockSkew = clockSkew, //過期時間容錯值，解決伺服器端時間不同步問題（秒）
                        RequireExpirationTime = true,
                    };
                });
        }
    }
}
