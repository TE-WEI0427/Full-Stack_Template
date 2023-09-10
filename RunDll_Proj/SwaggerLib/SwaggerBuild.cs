using Microsoft.AspNetCore.Builder;

namespace SwaggerLib
{
    public static class SwaggerBuild
    {
        /// <summary>
        /// WebApplication Use Swagger & UI
        /// </summary>
        /// <param name="app">Web 應用程序</param>
        public static void UseSwaggerPage(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        /// <summary>
        /// Use Swagger with xml file，
        /// 記得要於 csproj 檔案裡加上以下兩段，目的是要透過編譯器產生文件檔案
        /// ＜GenerateDocumentationFile＞true＜/GenerateDocumentationFile＞
        /// ＜NoWarn＞$(NoWarn);1591＜/NoWarn＞
        /// </summary>
        /// <param name="app">Web 應用程序</param>
        /// <param name="Title">專案名稱</param>
        /// <param name="Version">版本號</param>
        public static void UseSwaggerPageWithDoc(this WebApplication app, string Title, string Version)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/" + Version + "/swagger.json", Title + " " + Version);
                options.RoutePrefix = string.Empty;
            });
        }
    }
}
