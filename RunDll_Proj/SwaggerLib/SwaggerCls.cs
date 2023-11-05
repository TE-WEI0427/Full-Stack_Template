using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace SwaggerLib
{
    public static class SwaggerCls
    {
        /// <summary>
        /// 執行 Swagger 所有初始設定
        /// </summary>
        /// <param name="builder">指定服務描述項集合的合約</param>
        public static void SwaggerBuilderInit(this WebApplicationBuilder builder)
        {
            builder.AddJwtSwagger();
        }

        /// <summary>
        /// 執行 Swagger 所有初始設定，包含產生 xml 檔
        /// </summary>
        /// <param name="builder">指定服務描述項集合的合約</param>
        /// <param name="Title">專案名稱</param>
        /// <param name="Version">版本號</param>
        public static void SwaggerBuilderInit(this WebApplicationBuilder builder, string Title, string Version)
        {
            builder.AddJwtSwagger();
            builder.AddSwaggerDoc(Title, Version);
        }

        /// <summary>
        /// App 建置後，執行 Swagger 所有初始設定
        /// </summary>
        /// <param name="app">Web 應用程序</param>
        public static void SwaggerAppInit(this WebApplication app)
        {
            app.UseSwaggerPage();
        }

        /// <summary>
        /// App 建置後，執行 Swagger 所有初始設定，包含產生 xml 檔
        /// </summary>
        /// <param name="app">Web 應用程序</param>
        /// <param name="Title">專案名稱</param>
        /// <param name="Version">版本號</param>
        public static void SwaggerAppInit(this WebApplication app, string RoutePrefix, string Title, string Version)
        {  
            app.UseSwaggerPageWithDoc(RoutePrefix, Title, Version);
        }
    }
}
