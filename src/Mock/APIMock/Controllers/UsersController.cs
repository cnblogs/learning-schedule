using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Bogus;
using Cnblogs.Academy.DTO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace APIMock.Controllers
{
    public class UsersController : ControllerBase
    {
        [Route("users/cookie")]
        public async Task GetCookie(
            string loginName,
            bool isPersistent,
            [FromServices] IOptions<CookieAuthenticationOptions> cookieAuthOptions)
        {
            var claimsIdentity = new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, loginName) }, "Basic");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                claimsPrincipal,
                new AuthenticationProperties
                {
                    IsPersistent = isPersistent,
                    ExpiresUtc = DateTimeOffset.Now.Add(cookieAuthOptions.Value.ExpireTimeSpan)
                });
        }

        [Route("api/v2/users/[[userIds]]/loginNames")]
        public IActionResult GetLoginNames()
        {
            var faker = new Faker<UserDto>()
            .RuleFor(x=>x.LoginName, f=>f.Name.FirstName());
            return Ok(faker.Generate(2).Select(x => x.LoginName));
        }

        [Route("api/v2/user/{*url}")]
        [Route("api/v2/users/{*url}")]
        public ActionResult<UserDto> GetUser()
        {
            var faker = new Faker<UserDto>()
                .RuleFor(x => x.Alias, f => f.Name.FirstName())
                .RuleFor(x => x.Email, f => f.Internet.Email())
                .RuleFor(x => x.Remark, f => f.Lorem.Sentence())
                .RuleFor(x => x.Status, AccountStatus.Normal)
                .RuleFor(x => x.Wealth, f => f.Random.Int(0, 600))
                .RuleFor(x => x.BlogApp, f => f.Internet.DomainWord())
                .RuleFor(x => x.AvatarName, f => f.Internet.Avatar())
                .RuleFor(x => x.BlogId, f => f.Random.Int(0, 900000))
                .RuleFor(x => x.DisplayName, f => f.Name.FullName())
                .RuleFor(x => x.FollowerCount, f => f.Random.Int(0, 30000))
                .RuleFor(x => x.FollowingCount, f => f.Random.Int(0, 1000))
                .RuleFor(x => x.LoginName, f => f.Internet.UserName())
                .RuleFor(x => x.NotificationEmail, f => f.Internet.Email())
                .RuleFor(
                    x => x.NotificationType,
                    f => f.Random.Enum(NotificationType.Mail, NotificationType.Mail, NotificationType.SiteMessage))
                .RuleFor(x => x.UserId, f => f.Random.Guid())
                .RuleFor(x => x.SpaceUserId, x => x.Random.Int(0))
                .RuleFor(x => x.RegisterTime, f => f.Date.Past(3));

            return faker.Generate();
        }
    }
}
