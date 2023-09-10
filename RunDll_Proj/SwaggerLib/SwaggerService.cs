using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.Filters;
using System.Reflection;

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

        /// <summary>
        /// swagger 產生 xml 檔案，
        /// 使用後可以讀取我們所寫的註解
        /// </summary>
        /// <param name="builder">指定服務描述項集合的合約</param>
        /// <param name="Title">專案名稱</param>
        /// <param name="Version">版本號</param>
        public static void AddSwaggerDoc(this WebApplicationBuilder builder, string Title, string Version)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            // 讀取 swagger 產生的 xml 檔案 => 目的是為了可以讀取我們所寫的註解
            builder.Services.AddSwaggerGen(c =>
            {
                var xmlFilename = $"{Title}.xml";
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

                c.SwaggerDoc(Version, new OpenApiInfo
                {
                    Title = Title,
                    Version = Version
                });

                //c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                //{
                //    { new OpenApiSecurityScheme(){ }, new List<string>() }
                //});

                // swagger 版本控制 https://www.cnblogs.com/wei325/p/16047807.html
            });

        }
    }
}
