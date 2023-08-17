using JwtLib;
using SwaggerLib;

namespace Infrastructure
{
    // (J-2) Program
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
        }

        /// <summary>
        /// set service
        /// </summary>
        /// <param name="builder">指定服務描述項集合的合約</param>
        public static void SetService(this WebApplicationBuilder builder)
        {
            // (sw-1)
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
        /// app Builder
        /// </summary>
        /// <param name="app">Web 應用程序</param>
        public static void AppBuilder(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                // (sw-2)
                app.UseSwaggerPage();
            }
        }
    }
}
