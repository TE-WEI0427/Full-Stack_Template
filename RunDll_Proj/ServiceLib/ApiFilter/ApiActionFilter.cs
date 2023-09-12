using Microsoft.AspNetCore.Mvc.Filters;

// ref : https://www.cnblogs.com/code4nothing/p/build-todolist-12.html

namespace ApiFilter
{
    /// <summary>
    /// API 過濾器
    /// </summary>
    public interface IApiActionFilterAttribute : IAuthorizationFilter, IActionFilter, IResultFilter, IExceptionFilter, IResourceFilter
    {// ActionFilterAttribute

        // Execut 1
        public new virtual void OnAuthorization(AuthorizationFilterContext context)
        {
            
        }

        // Execut 2
        public new virtual void OnResourceExecuting(ResourceExecutingContext context)
        {

        }

        // Execut 3
        public new virtual void OnActionExecuting(ActionExecutingContext context)
        {

        }

        // Execut 4
        public new virtual void OnActionExecuted(ActionExecutedContext context)
        {

        }

        // if Exception Execut
        public new virtual void OnException(ExceptionContext context)
        {

        }

        // Execut 5
        public new virtual void OnResultExecuting(ResultExecutingContext context)
        {

        }

        // Execut 6
        public new virtual void OnResultExecuted(ResultExecutedContext context)
        {

        }

        // Execut 7
        public new virtual void OnResourceExecuted(ResourceExecutedContext context)
        {

        }
    }
}
