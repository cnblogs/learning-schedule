using System;
using System.Security.Claims;
using Cnblogs.Academy.DTO;

namespace Cnblogs.Academy.ServiceAgent.UCenterService
{
    public static class ClaimsPrincipalExtensions
    {
        public static UserDto UCenter(this ClaimsPrincipal claimsPrincipal)
        {
            if(claimsPrincipal == null)
            {
                throw new ArgumentNullException(nameof(claimsPrincipal));
            }

            if(!claimsPrincipal.Identity.IsAuthenticated)
            {
                return null;
            }

            var principal = claimsPrincipal as UCenterClaimsPrincipal;
            if(principal == null)
            {
                throw new Exception("Please add services.AddUCenter() and app.UseUCenter() in Startup");
            }

            return principal.User;
        }
    }

}
