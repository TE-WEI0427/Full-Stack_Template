using Microsoft.AspNetCore.Mvc.Filters;

namespace Infrastructure.ActionFilter
{
    public class ApiActionFilter : ActionFilterAttribute, IAuthorizationFilter, IResourceFilter, IExceptionFilter
    {
        // Execut 1
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string actionName = context.GetType().Name;
        }

        // Execut 2
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            string actionName = context.GetType().Name;
        }

        // Execut 3
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
        }

        // Execut 4
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
        }

        // if Exception Execut
        public void OnException(ExceptionContext context)
        {
            string actionName = context.GetType().Name;
        }

        // Execut 5
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            base.OnResultExecuting(context);
        }

        // Execut 6
        public override void OnResultExecuted(ResultExecutedContext context)
        {
            base.OnResultExecuted(context);
        }

        // Execut 7
        public void OnResourceExecuting(ResourceExecutedContext context)
        {
            string actionName = context.GetType().Name;
        }

        // Execut 8
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            string actionName = context.GetType().Name;
        }
    }
}
