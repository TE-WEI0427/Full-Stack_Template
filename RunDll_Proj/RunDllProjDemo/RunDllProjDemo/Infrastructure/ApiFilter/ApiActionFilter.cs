using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

using Newtonsoft.Json;

using BaseLib;
using BasicConfig;
using System.Text;
using ApiFilter;

namespace Infrastructure.ApiFilter
{
    /// <summary>
    /// API 過濾器
    /// (Service-Filter-1)
    /// </summary>
    public class ApiActionFilterAttribute : IApiActionFilterAttribute
    {
        private readonly IWebHostEnvironment _environment; // 取得執行環境

        public ApiActionFilterAttribute(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        #region Action Filter
        /// <summary>
        /// OnAuthorization
        /// </summary>
        /// <param name="context"></param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            StringBuilder logStr = new();
            logStr.AppendLine("----------------------------------------------------------------------------------");
            logStr.Append(string.Format("[{0}] OnAuthorization", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            CommonIO.WriteLog(Path.Combine(_environment.WebRootPath, "Log"), DateTime.Now.ToString("yyyy-MM-dd") + "_log.txt", logStr.ToString());
        }
        /// <summary>
        /// OnResourceExecuting
        /// </summary>
        /// <param name="context"></param>
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            string logStr = string.Format("[{0}] OnResourceExecuting", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            CommonIO.WriteLog(Path.Combine(_environment.WebRootPath, "Log"), DateTime.Now.ToString("yyyy-MM-dd") + "_log.txt", logStr);
        }
        /// <summary>
        /// OnActionExecuting        
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            string controllerName = ((ControllerBase)context.Controller).ControllerContext.ActionDescriptor.ControllerName;
            string actionName = ((ControllerBase)context.Controller).ControllerContext.ActionDescriptor.ActionName;
            string requestBody = (context.ActionDescriptor is ControllerActionDescriptor) ? JsonConvert.SerializeObject(context.ActionArguments) : string.Empty;

            string logStr = string.Format("[{0}] Controller【{1}】Action【{2}】: OnActionExecuting | Input : {3}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), controllerName, actionName, requestBody);
            CommonIO.WriteLog(Path.Combine(_environment.WebRootPath, "Log"), DateTime.Now.ToString("yyyy-MM-dd") + "_log.txt", logStr);
        }
        /// <summary>
        /// OnActionExecuted
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            string controllerName = ((ControllerBase)context.Controller).ControllerContext.ActionDescriptor.ControllerName;
            string actionName = ((ControllerBase)context.Controller).ControllerContext.ActionDescriptor.ActionName;
            string logStr = string.Format("[{0}] Controller【{1}】Action【{2}】: OnActionExecuted", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), controllerName, actionName);
            CommonIO.WriteLog(Path.Combine(_environment.WebRootPath, "Log"), DateTime.Now.ToString("yyyy-MM-dd") + "_log.txt", logStr);
        }
        /// <summary>
        /// OnResultExecuting
        /// </summary>
        /// <param name="context"></param>
        public void OnResultExecuting(ResultExecutingContext context)
        {
            string controllerName = ((ControllerBase)context.Controller).ControllerContext.ActionDescriptor.ControllerName;
            string actionName = ((ControllerBase)context.Controller).ControllerContext.ActionDescriptor.ActionName;

            Result? result = (context.Result as OkObjectResult)?.Value as Result;
            string resultCode = result != null ? Enum.GetName(result.ResultCode) ?? "" : "";
            string message = result != null ? result.Message ?? "" : "";
            string jsonData = result != null ? result.JsonData() ?? "" : "";

            string logStr = 
                string.Format(
                    "[{0}] Controller【{1}】Action【{2}】: OnResultExecuting | Result : ResultCode【{3}】，Massage【{4}】，Data【{5}】",
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), 
                    controllerName, 
                    actionName,
                    resultCode,
                    message,
                    jsonData);
            CommonIO.WriteLog(Path.Combine(_environment.WebRootPath, "Log"), DateTime.Now.ToString("yyyy-MM-dd") + "_log.txt", logStr);
        }
        /// <summary>
        /// OnResultExecuted
        /// </summary>
        /// <param name="context"></param>
        public void OnResultExecuted(ResultExecutedContext context)
        {
            string controllerName = ((ControllerBase)context.Controller).ControllerContext.ActionDescriptor.ControllerName;
            string actionName = ((ControllerBase)context.Controller).ControllerContext.ActionDescriptor.ActionName;
            string logStr = string.Format("[{0}] Controller【{1}】Action【{2}】: OnResultExecuted", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), controllerName, actionName);
            CommonIO.WriteLog(Path.Combine(_environment.WebRootPath, "Log"), DateTime.Now.ToString("yyyy-MM-dd") + "_log.txt", logStr);
        }
        /// <summary>
        /// OnException
        /// </summary>
        /// <param name="context"></param>
        public void OnException(ExceptionContext context)
        {
            string logStr = string.Format("[{0}] OnException | Message【{1}】", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), context.Exception.Message);
            CommonIO.WriteLog(Path.Combine(_environment.WebRootPath, "Log"), DateTime.Now.ToString("yyyy-MM-dd") + "_log.txt", logStr);
        }
        /// <summary>
        /// OnResourceExecuted
        /// </summary>
        /// <param name="context"></param>
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            StringBuilder logStr = new();
            logStr.AppendLine(string.Format("[{0}] OnResourceExecuted", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            logStr.Append("----------------------------------------------------------------------------------");

            CommonIO.WriteLog(Path.Combine(_environment.WebRootPath, "Log"), DateTime.Now.ToString("yyyy-MM-dd") + "_log.txt", logStr.ToString());
        }
        #endregion
    }
}
