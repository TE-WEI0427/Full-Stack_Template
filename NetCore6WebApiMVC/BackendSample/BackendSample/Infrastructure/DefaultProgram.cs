﻿using JwtLib;
using MailLib;
using ServiceLib;
using SwaggerLib;

namespace Infrastructure
{
    // (2) Program
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

            // mailConfig
            MailConfig.Host = builder.Configuration["MailConfig:Host"];
            MailConfig.Port = Convert.ToInt32(builder.Configuration["MailConfig:Port"]);
            MailConfig.MailAccount = builder.Configuration["MailConfig:MailAccount"];
            MailConfig.MailPassword = builder.Configuration["MailConfig:MailPassword"];
            MailConfig.SecureSocketOptions = Convert.ToInt32(builder.Configuration["MailConfig:SecureSocketOptions"]);
            MailConfig.MailDisplayName = builder.Configuration["MailConfig:MailDisplayName"];
        }

        /// <summary>
        /// set service
        /// </summary>
        /// <param name="builder">指定服務描述項集合的合約</param>
        public static void SetService(this WebApplicationBuilder builder)
        {
            builder.AddJwtSwagger();
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
            // (Ser-1)
            builder.AddServiceScoped();
        }

        /// <summary>
        /// app Builder
        /// </summary>
        /// <param name="app">Web 應用程序</param>
        public static void AppBuilder(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerPage();
            }
        }
    }
}
