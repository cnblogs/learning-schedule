using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Cnblogs.Academy.ServiceAgent.UCenterService
{
    public class UCenterMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IUCenterService _ucenterService;
        private readonly ILogger _logger;

        public UCenterMiddleware(RequestDelegate next, IUCenterService ucenterService, ILoggerFactory loggerFactory)
        {
            _next = next;
            _ucenterService = ucenterService;
            _logger = loggerFactory.CreateLogger<UCenterMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                var user = await _ucenterService.GetUser(u => u.LoginName, context.User.Identity.Name);
                if (user == null)
                {
                    _logger.LogInformation($"Account does not exist or is disabled for '{context.User.Identity.Name}' login name");
                    context.Response.StatusCode = 401;
                    return;
                }

                context.User = new UCenterClaimsPrincipal(context.User, user);
            }

            await _next(context);
        }

    }
}
