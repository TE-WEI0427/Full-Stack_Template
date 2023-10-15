using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Cors;

using BasicConfig;
using Service;

namespace Infrastructure
{
    /// <summary>
    /// Api 基底元件
    /// </summary>
    [EnableCors("_demoAllowSpecificOrigins")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DefaultController : ControllerBase, IActionFilter, IResultFilter
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
        /// UserData
        /// </summary>
        /// <value></value>
        public AppUserData UserData
        {
            get
            {
                return new()
                {
                    SysUserId = int.TryParse(this.UserService?.GetUserInfo("SysUserId"), out int val_sysUserId) ? val_sysUserId : 0,
                    Platform = this.UserService?.GetUserInfo("Platform") ?? "",
                    Model = this.UserService?.GetUserInfo("Model") ?? "",
                    UUID = this.UserService?.GetUserInfo("UUID") ?? "",
                    Memo = this.UserService?.GetUserInfo("Memo") ?? "",
                    ClientIp = this.UserService?.GetUserInfo("ClientIp") ?? ""
                };
            }
        }
        /// <summary>
        /// API 呼叫紀錄
        /// </summary>
        /// <value></value>
        public ILogService LogService { get; private set; }
        #region 建構子
        /// <summary>
        /// Api 基底元件 建構子 用於會產生JWT的API
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="userService"></param>
        /// <param name="logService"></param>
        public DefaultController(IConfiguration configuration, IUserService userService, ILogService logService)
        {
            this.Configuration = configuration;
            this.UserService = userService;
            this.LogService = logService;
        }
        #endregion

        #region filter
        /// <summary>
        /// parameter
        /// </summary>
        IDictionary<string, object?>? dict_parametersInfo;

        /// <summary>
        /// OnActionExecuting
        /// </summary>
        /// <param name="context"></param>
        [NonAction]
        public void OnActionExecuting(ActionExecutingContext context)
        {
            //string parametersInfo = JsonConvert.SerializeObject(context.ActionArguments);

            dict_parametersInfo = context.ActionArguments;

            if (dict_parametersInfo.Count > 0)
            {
                _ = LogService.AppStartLog(
                    dict_parametersInfo.TryGetValue("model", out object? model) ? model : dict_parametersInfo,
                    this.UserData, WebApiLogActType.Start);
            }
            else
            {
                _ = LogService.AppStartLog(this.UserData, WebApiLogActType.Start);
            }
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
            var jsonResult = context.Result as OkObjectResult;
            Result? resultInfo = jsonResult?.Value as Result;
            //string json = resultInfo?.JsonData() ?? "";

            if (dict_parametersInfo?.Count > 0)
            {
                _ = LogService.AppEndLog(
                    this.dict_parametersInfo.TryGetValue("model", out object? model) ? model : this.dict_parametersInfo,
                    this.UserData, resultInfo ?? new Result(), WebApiLogActType.End);
            }
            else
            {
                _ = LogService.AppEndLog(this.UserData, resultInfo ?? new Result(), WebApiLogActType.End);
            }
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
