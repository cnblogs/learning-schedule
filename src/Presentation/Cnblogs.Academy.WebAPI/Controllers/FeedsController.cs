using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cnblogs.Academy.Application.FeedsAppService;
using Cnblogs.Common;
using Cnblogs.Feed.DTO;
using Cnblogs.UCenter.ServiceAgent;
using Cnblogs.URelation.DTO;
using Cnblogs.URelation.ServiceAgent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cnblogs.Academy.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class FeedsController : AcademyControllerBase
    {
        private readonly IFeedsAppService _feedsAppSvc;
        private readonly IUCenterService _uCenterSvc;
        private readonly IUserRelationGroupService _groupSvc;

        public FeedsController(IFeedsAppService feedsAppSvc, IUCenterService uCenterSvc, IUserRelationGroupService groupSvc)
        {
            _groupSvc = groupSvc;
            _uCenterSvc = uCenterSvc;
            _feedsAppSvc = feedsAppSvc;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IEnumerable<FeedDto>> Get(int page = 1, int size = 10)
        {
            return await _feedsAppSvc.GetAcademyFeedsAsync(page, size);
        }

        [AllowAnonymous]
        [HttpGet("{alias}")]
        public async Task<ActionResult<PagedResult<FeedDto>>> GetFeeds(string alias, int page = 1, int size = 10, bool guest = false, bool myself = false)
        {
            if (guest)
            {
                return await _feedsAppSvc.GetFeeds(alias, page, size);
            }
            else
            {
                if (!IsAuthenticated)
                {
                    return Unauthorized();
                }

                if (UCenterUser.Alias == alias)
                {
                    if (myself)
                    {
                        return await _feedsAppSvc.GetFeeds(alias, page, size, true);
                    }
                    return await _feedsAppSvc.GetConcernFeeds(page, size, UCenterUser.UserId);
                }
                return PagedResult<FeedDto>.Empty();
            }
        }

        [AllowAnonymous]
        [HttpGet("user/{alias}")]
        public async Task<IActionResult> GetUser(string alias)
        {
            var user = await _uCenterSvc.GetUser(x => x.Alias, alias);
            if (user == null) return null;
            return new JsonResult(new
            {
                name = user.DisplayName,
                avatar = user.AvatarName
            });
        }

        [HttpGet("groups")]
        public async Task<IActionResult> GetGroups()
        {
            var groups = await _groupSvc.GetMyGroupsAsync(UCenterUser.UserId);
            return new JsonResult(groups.Select(x => new { x.Id, x.Name }));
        }
    }
}
