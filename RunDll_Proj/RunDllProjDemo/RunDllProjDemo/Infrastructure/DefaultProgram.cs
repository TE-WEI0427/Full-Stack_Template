using JwtLib;
using MailLib;
using ServiceLib;
using SqlLib.SqlTool;
using SwaggerLib;
using Infrastructure.ActionFilter;
using System.Reflection;

namespace Infrastructure
{
    /// <summary>
    /// Api 基底元件
    /// (Jwt-2) Program
    /// </summary>
    public static class DefaultProgram
    {
        /// <summary>
        /// set config
        /// </summary>
        /// <param name="builder">指定服務描述項集合的合約</param>
        public static void SetConfig(this WebApplicationBuilder builder)
        {
            // JwtSettings
            JwtSettings.Issuer = builder.Configuration["JwtSettings:Issuer"];
            JwtSettings.Audience = builder.Configuration["JwtSettings:Audience"];
            JwtSettings.SecretKey = builder.Configuration["JwtSettings:SecretKey"];

            // (Mail-2) mailConfig
            MailConfig.Host = builder.Configuration["MailConfig:Host"];
            MailConfig.Port = Convert.ToInt32(builder.Configuration["MailConfig:Port"]);
            MailConfig.MailAccount = builder.Configuration["MailConfig:MailAccount"];
            MailConfig.MailPassword = builder.Configuration["MailConfig:MailPassword"];
            MailConfig.SecureSocketOptions = Convert.ToInt32(builder.Configuration["MailConfig:SecureSocketOptions"]);
            MailConfig.MailDisplayName = builder.Configuration["MailConfig:MailDisplayName"];

            // (SQL-2) SqlSetting
            SqlSetting.StrConnection1 = builder.Configuration["ConnectionStrings:StrConnection1"];
            SqlSetting.StrConnection2 = builder.Configuration["ConnectionStrings:StrConnection2"];
            SqlSetting.StrConnection3 = builder.Configuration["ConnectionStrings:StrConnection3"];
        }

        /// <summary>
        /// set service
        /// </summary>
        /// <param name="builder">指定服務描述項集合的合約</param>
        public static void SetService(this WebApplicationBuilder builder)
        {
            // (Swag-1)
            builder.AddJwtSwagger();
            builder.AddSwaggerDoc(Assembly.GetExecutingAssembly().GetName().Name ?? "swaggerDoc", "v1");

            builder.AddJwtAuthentication(TimeSpan.FromMinutes(5));
        }

        /// <summary>
        /// set cors
        /// </summary>
        /// <param name="builder">指定服務描述項集合的合約</param>
        public static void SetCors(this WebApplicationBuilder builder)
        {
            string DemoAllowSpecificOrigins = "_demoAllowSpecificOrigins";

            builder.Services
               .AddCors(options =>
               {
                   options.AddPolicy(name: DemoAllowSpecificOrigins,
                                     policy =>
                                     {
                                         //policy.WithOrigins(builder.Configuration.GetSection("AllowOrigins").Get<string[]>());
                                         //policy.WithOrigins("http://localhost"
                                         //    , "http://localhost:191"
                                         //    , "http://localhost:194"
                                         //    , "https://localhost:44388/"
                                         //    , "http://www.zhtech.com.tw:191"
                                         //    , "http://www.zhtech.com.tw:194")
                                         //.SetIsOriginAllowed(oring => true)
                                         policy.AllowAnyOrigin();  // Access-Control-Allow-Origin: *
                                         policy.AllowAnyHeader();
                                         policy.AllowAnyMethod();
                                         //.AllowCredentials();
                                     });
               });
        }

        /// <summary>
        /// Set Scoped
        /// </summary>
        /// <param name="builder">指定服務描述項集合的合約</param>
        public static void SetScoped(this WebApplicationBuilder builder)
        {
            // (Service-1)
            builder.AddServiceScoped();

            builder.Services.AddScoped<ApiActionFilter>();
        }

        /// <summary>
        /// app Builder
        /// </summary>
        /// <param name="app">Web 應用程序</param>
        public static void AppBuilder(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                // (Swag-2)
                //app.UseSwaggerPage();
                app.UseSwaggerPageWithDoc(Assembly.GetExecutingAssembly().GetName().Name ?? "swaggerDoc", "v1");
            }
        }
    }
}
