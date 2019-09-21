using System.Security.Claims;
using Cnblogs.Academy.DTO;

namespace Cnblogs.Academy.ServiceAgent.UCenterService
{
    public class UCenterClaimsPrincipal : ClaimsPrincipal
    {
        public UserDto User { get; private set; }

        public UCenterClaimsPrincipal(ClaimsPrincipal claimsPrincipal, UserDto user)
            : base(claimsPrincipal)
        {
            User = user;
        }
    }
}
