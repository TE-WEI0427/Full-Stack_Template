using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

using Service;
using BaseLib;
using BasicConfig;

namespace Infrastructure
{
    /// <summary>
    /// Api 基底元件 自訂 API Filter
    /// </summary>
    [EnableCors("_demoAllowSpecificOrigins")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CusApiFilterController : ControllerBase, IActionFilter, IResultFilter
    {
        /// <summary>
        /// 表示一組索引鍵/值應用程式組態屬性
        /// </summary>
        /// <value></value>
        public IConfiguration? Configuration { get; private set; }
        /// <summary>
        /// 用戶服務
        /// </summary>
        /// <value></value>
        public IUserService? UserService { get; private set; }
        /// <summary>
        /// 取得執行環境
        /// </summary>
        public IWebHostEnvironment Environment { get; private set; }
        /// <summary>
        /// UserData
        /// </summary>
        /// <value></value>
        public DefalutUserData UserData
        {
            get
            {
                return new()
                {
                    SysUserId = Convert.ToInt32(this.UserService?.GetUserInfo("SysUserId")),
                    Role = this.UserService?.GetUserInfo("Role") ?? "",
                    memo = this.UserService?.GetUserInfo("memo") ?? "",
                };
            }
        }
        #region 建構子
        /// <summary>
        /// Api 基底元件 建構子
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="userService"></param>
        public CusApiFilterController(IConfiguration configuration, IUserService userService, IWebHostEnvironment environment)
        {
            this.Configuration = configuration;
            this.UserService = userService;
            this.Environment = environment;
        }
        #endregion

        #region filter
        /// <summary>
        /// OnActionExecuting
        /// </summary>
        /// <param name="context"></param>
        [NonAction]
        public void OnActionExecuting(ActionExecutingContext context)
        {
            string controllerName = ((ControllerBase)context.Controller).ControllerContext.ActionDescriptor.ControllerName;
            string actionName = ((ControllerBase)context.Controller).ControllerContext.ActionDescriptor.ActionName;
            string requestBody = (context.ActionDescriptor is ControllerActionDescriptor) ? JsonConvert.SerializeObject(context.ActionArguments) : string.Empty;

            string logStr = string.Format("[{0}] Controller【{1}】Action【{2}】: OnActionExecuting | Input : {3}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), controllerName, actionName, requestBody);
            CommonIO.WriteLog(Path.Combine(this.Environment.WebRootPath, "Log"), DateTime.Now.ToString("yyyy-MM-dd") + "_log.txt", logStr);
        }

        /// <summary>
        /// OnActionExecuted
        /// </summary>
        /// <param name="context"></param>
        [NonAction]
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // no action
        }

        /// <summary>
        /// OnResultExecuting
        /// </summary>
        /// <param name="context"></param>
        [NonAction]
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
            CommonIO.WriteLog(Path.Combine(this.Environment.WebRootPath, "Log"), DateTime.Now.ToString("yyyy-MM-dd") + "_log.txt", logStr);
        }

        /// <summary>
        /// OnResultExecuted
        /// </summary>
        /// <param name="context"></param>
        [NonAction]
        public void OnResultExecuted(ResultExecutedContext context)
        {
            // no action
        }
        #endregion
    }
}
