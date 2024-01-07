using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using pkaselj_lab_07_.Exceptions;
using System.Net;

namespace pkaselj_lab_07_.Filters
{
    public class LogFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine($"Sent response at: {DateTime.UtcNow.ToLongTimeString()}");
            
            // If error is thrown
            if(context.Exception != null && !context.ExceptionHandled)
            {
                Console.WriteLine($"ERROR: {context.Exception.Message}");
                context.ExceptionHandled = true;

                // This is a security feature that only passes custom exception
                // type (UserErrorMessage) to the user. If any other exception
                // makes it here, it will not be sent to the user, instead,
                // the user will get a generic error message ("Web API encountered an error!")
                // This prevents hackers to gather information about the internals
                // of our application
                string errorMessage;
                int statusCode;
                if(context.Exception.GetType() == typeof(UserErrorMessage))
                {
                    // User error
                    statusCode = (int)HttpStatusCode.BadRequest;
                    errorMessage = context.Exception.Message;
                }
                else
                {
                    // Server error
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    errorMessage = "Web API encountered an error!";
                }

                // Create the error message
                context.Result = new ContentResult {
                    StatusCode = statusCode,
                    ContentType = "application/text",
                    Content = errorMessage,
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
