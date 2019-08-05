using System.Threading.Tasks;
using Cnblogs.Academy.Application.ScheduleAppService;
using Cnblogs.Academy.Common;
using Cnblogs.CAP.RedisUtility;
using Cnblogs.Academy.ServiceAgent.HotCommentApi;
using Cnblogs.Academy.WebAPI.Utils;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;

namespace Cnblogs.Academy.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class ThumbController : AcademyControllerBase
    {
        private readonly IThumbupApiService _thumbupApiService;
        private readonly IScheduleService _scheduleService;
        private readonly RedisUtility _redisUtility;

        public ThumbController
        (
            IThumbupApiService thumbupApiService,
            IScheduleService scheduleService,
            RedisUtility redisUtility
        )
        {
            _thumbupApiService = thumbupApiService;
            _scheduleService = scheduleService;
            _redisUtility = redisUtility;
        }

        [HttpPost("up")]
        public async Task<BooleanResult> ThumbUp(long objectId)
        {
            var result = await
            _thumbupApiService.UpAsync(new ThumbupInput(objectId.ToString(), HttpContext.GetUserIp(), UserId));
            if (result.Success && result.Value == 0)
            {
                await Cancel(objectId);
                return BooleanResult<int>.Succeed(-1);
            }
            return result;
        }

        [CapSubscribe("Academy.ItemDoneRecord.Thumbup")]
        public async Task HandleThumbup([FromBody]ThumbupEvent e)
        {
            if (await _redisUtility.DistinctAsync("academy.itemdonerecord.thumbup" + e.Id))
            {
                await _scheduleService.IncreaseLikeCountAsync(e.ObjectId);
            }
        }

        [HttpDelete("cancellation")]
        public async Task Cancel(long objectId)
        {
            await
            _thumbupApiService.CancelAsync(new ThumbCancellationInput(objectId.ToString(), HttpContext.GetUserIp(), UserId));
        }

        [CapSubscribe("Academy.ItemDoneRecord.ThumbupCancellation")]
        public async Task HandleThumbupCancellation([FromBody]ThumbupCancellationEvent e)
        {
            var key = "-academy.itemdonerecord.thumbup" + e.Id;
            if (await _redisUtility.DistinctAsync(key))
            {
                await _scheduleService.IncreaseLikeCountAsync(e.ObjectId, -1);
            }
        }
    }
}
