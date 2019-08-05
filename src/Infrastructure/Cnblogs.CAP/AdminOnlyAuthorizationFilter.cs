using System.Collections.Concurrent;
using System.Linq;
using Cnblogs.Academy.ServiceAgent.UCenterService;
using DotNetCore.CAP.Dashboard;
using Microsoft.Extensions.DependencyInjection;

namespace Cnblogs.CAP
{
    public class AdminOnlyAuthorizationFilter : IDashboardAuthorizationFilter
    {
        private readonly BlockingCollection<string> _adminNames = new BlockingCollection<string>();
        private readonly bool _isDevelopment;

        public AdminOnlyAuthorizationFilter(bool isDevelopment)
        {
            _isDevelopment = isDevelopment;
        }

        public bool Authorize(DashboardContext context)
        {
            if (_isDevelopment)
            {
                var filter = new LocalRequestsOnlyAuthorizationFilter();
                if (filter.Authorize(context))
                {
                    return true;
                }
            }

            var capContext = context as CapDashboardContext;
            var httpContext = capContext?.HttpContext;
            if (httpContext == null) return false;
            var loginName = httpContext.User.Identity.Name;
            if (string.IsNullOrEmpty(loginName)) return false;

            if (_adminNames.Contains(loginName)) return true;

            var svc = httpContext.RequestServices.GetService<IUCenterService>();
            var task = svc.CheckUserIsManager(loginName);
            task.Wait();
            var ok = task.Result;
            if (ok)
            {
                _adminNames.TryAdd(loginName);
            }

            return ok;
        }
    }
}
