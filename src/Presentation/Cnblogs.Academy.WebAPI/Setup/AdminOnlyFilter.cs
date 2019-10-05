using System;
using System.Linq;
using System.Threading.Tasks;
using Cnblogs.Academy.ServiceAgent.UCenterService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Cnblogs.Academy.WebAPI.Setup
{
    public class AdminOnlyAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            foreach (var desc in context.ActionDescriptor.FilterDescriptors)
            {
                if (desc.Filter.GetType() == typeof(AllowAnonymousFilter))
                {
                    await next();
                    return;
                }
            }
            var name = context.HttpContext.User.Identity.Name;
            if (!string.IsNullOrEmpty(name))
            {
                var svc = context.HttpContext.RequestServices.GetService<IUCenterService>();
                var isAdmin = await svc.CheckUserIsManager(name);
                if (isAdmin)
                {
                    await next();
                    return;
                }
            }
            context.Result = new UnauthorizedObjectResult("Not allowed");
        }
    }
}
