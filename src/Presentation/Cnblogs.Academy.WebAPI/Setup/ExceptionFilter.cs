using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Cnblogs.Academy.WebAPI.Setup
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is ValidationException)
            {
                var result = new ObjectResult(context.Exception.Message);
                result.StatusCode = (int)HttpStatusCode.Forbidden;
                context.Result = result;
            }
        }
    }
}
