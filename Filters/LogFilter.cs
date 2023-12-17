using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace pkaselj_lab_07_.Filters
{
    public class LogFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine($"Sent response at: {DateTime.UtcNow.ToLongTimeString()}");
            
            if(context.Exception != null && !context.ExceptionHandled)
            {
                Console.WriteLine($"ERROR: {context.Exception.Message}");
                context.ExceptionHandled = true;

                context.Result = new ContentResult {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    ContentType = "application/text",
                    Content = "Web API encountered an error!",
                };
            }

            base.OnActionExecuted(context);
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine($"Got one request at: {DateTime.UtcNow.ToLongTimeString()}");

            base.OnActionExecuting(context);
        }
    }
}
