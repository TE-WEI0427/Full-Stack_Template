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
    }
}
