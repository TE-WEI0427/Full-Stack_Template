using Microsoft.AspNetCore.Mvc.Infrastructure;

using JwtLib;
using MailLib;
using Service;
using ServiceLib;
using SqlLib.SqlTool;
using SwaggerLib;
using System.Reflection;

namespace Infrastructure
{
    /// <summary>
    /// 預設執行 Program
    /// </summary>
    public static class DefaultProgram
    {
        /// <summary>
        /// 初始設定
        /// </summary>
        /// <param name="builder">指定服務描述項集合的合約</param>
        public static void Init(this WebApplicationBuilder builder)
        {
            // Jwt 參數設定與設定權限驗證
            builder.JwtBuilderInit();

            // Swagger 使用 JWT 驗證 並產生 Xml 檔
            builder.SwaggerBuilderInit(Assembly.GetExecutingAssembly().GetName().Name ?? "swaggerDoc", "v1");
        }

        /// <summary>
        /// set config
        /// </summary>
        /// <param name="builder">指定服務描述項集合的合約</param>
        public static void SetConfig(this WebApplicationBuilder builder)
        {
            // MailConfig
            MailConfig.Host = builder.Configuration["MailConfig:Host"];
            MailConfig.Port = Convert.ToInt32(builder.Configuration["MailConfig:Port"]);
            MailConfig.MailAccount = builder.Configuration["MailConfig:MailAccount"];
            MailConfig.MailPassword = builder.Configuration["MailConfig:MailPassword"];
            MailConfig.SecureSocketOptions = Convert.ToInt32(builder.Configuration["MailConfig:SecureSocketOptions"]);
            MailConfig.MailDisplayName = builder.Configuration["MailConfig:MailDisplayName"];

            // SqlSetting
            SqlSetting.StrConnection1 = builder.Configuration["ConnectionStrings:StrConnection1"];
            SqlSetting.StrConnection2 = builder.Configuration["ConnectionStrings:StrConnection2"];
            SqlSetting.StrConnection3 = builder.Configuration["ConnectionStrings:StrConnection3"];
        }

        /// <summary>
        /// set cors
        /// </summary>
        /// <param name="builder">指定服務描述項集合的合約</param>
        public static void SetCors(this WebApplicationBuilder builder)
        {// 要設定 Cors 才能使用 JWT

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
            // UserService HttpContextAccessor
            builder.AddServiceScoped();

            // LogService
            builder.Services.AddScoped<ILogService, LogService>();
            builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
        }

        /// <summary>
        /// app Builder
        /// </summary>
        /// <param name="app">Web 應用程序</param>
        public static void AppBuilder(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                // Swagger 使用產生的 Xml 檔 (顯示 summary 註釋)
                app.SwaggerAppInit(Assembly.GetExecutingAssembly().GetName().Name ?? "swaggerDoc", "v1");
            }
        }
    }
}
