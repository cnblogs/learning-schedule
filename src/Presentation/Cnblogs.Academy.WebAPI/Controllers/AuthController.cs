using System.Threading.Tasks;
using Cnblogs.Academy.Common;
using Cnblogs.Academy.Domain;
using Cnblogs.Academy.ServiceAgent.MsgApi;
using Cnblogs.Academy.ServiceAgent.UCenterService;
using Microsoft.AspNetCore.Mvc;

namespace Cnblogs.Academy.WebAPI.Controllers
{
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly IMsgApiService _msgApiService;

        public AuthController(IMsgApiService msgApiService)
        {
            _msgApiService = msgApiService;
        }

        [HttpGet]
        public bool Auth()
        {
            return User.Identity.IsAuthenticated;
        }

        [HttpGet("privacy")]
        public async Task<BooleanResult> Privacy()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.UCenter();
                var count = await _msgApiService.UnreadCount(user.SpaceUserId);
                return BooleanResult<Privacy>.Succeed(new Privacy(user.DisplayName, count, user.Alias, user.IconName));
            }
            else
            {
                return BooleanResult.Fail("未登录");
            }
        }

        [HttpGet("~/api/appId")]
        public string AppId()
        {
            return AppConst.AppId;
        }
    }

    class Privacy
    {
        public Privacy(string name, int unreadCount, string alias, string icon)
        {
            Name = name;
            UnreadCount = unreadCount;
            Alias = alias;
            Icon = icon;
        }

        public string Name { get; set; }
        public int UnreadCount { get; set; }
        public string Alias { get; set; }
        public string HomeUrl { get => Cnblogs.Domain.Abstract.HomeUrl.Of(Alias); }
        public string Icon { get; set; }
    }
}
