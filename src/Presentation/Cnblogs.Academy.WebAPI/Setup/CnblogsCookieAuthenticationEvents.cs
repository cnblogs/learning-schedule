using System.Net;
using System.Text;
using System.Threading.Tasks;
using Cnblogs.Academy.WebAPI.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace Cnblogs.Academy.WebAPI.Setup
{
    public class CnblogsCookieAuthenticationEvents : CookieAuthenticationEvents
    {
        public override async Task RedirectToLogin(RedirectContext<CookieAuthenticationOptions> context)
        {
            if (context.Request.IsAjaxRequest() || context.Request.Path.StartsWithSegments("/api"))
            {
                context.Response.StatusCode = 401;
                context.Response.ContentType = "text/plain;charset=utf-8";
                await context.Response.WriteAsync("需要登陆", Encoding.UTF8);
            }
            else
            {
                context.Response.Redirect("//account.cnblogs.com/signin?ReturnUrl=" + WebUtility.UrlEncode(context.Request.GetAbsoluteUri()));
            }
        }
    }
}
