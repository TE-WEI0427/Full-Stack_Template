using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.Filters;

namespace SwaggerLib
{
    public static class SwaggerService
    {
        /// <summary>
        /// Swagger 加入 Jwt 驗證
        /// </summary>
        /// <param name="builder">指定服務描述項集合的合約</param>
        public static void AddJwtSwagger(this WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    // 描述
                    Description = "Standard Authorization header using the Bearer scheme (\"Bearer {token}\")",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                options.OperationFilter<SecurityRequirementsOperationFilter>();
            });
        }
    }
}
