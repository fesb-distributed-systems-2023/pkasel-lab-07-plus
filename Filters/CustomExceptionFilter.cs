using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace pkaselj_lab_07_.Filters
{
    public class CustomExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            context.ExceptionHandled = true;
            context.HttpContext.Response.Clear();
            context.HttpContext.Response.StatusCode = 400;
            context.HttpContext.Response.ContentType = new MediaTypeHeaderValue("text/html").ToString();
            var message = context.Exception.Message;
            context.HttpContext.Response.BodyWriter.WriteAsync(Encoding.UTF8.GetBytes(message));

            Console.WriteLine(message);
            base.OnException(context);
        }
    }
}
