using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Cnblogs.Academy.Application.ScheduleAppService;
using Cnblogs.CAP.RedisUtility;
using Cnblogs.Academy.ServiceAgent.HotCommentApi;
using Cnblogs.Academy.WebAPI.Utils;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cnblogs.Academy.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class CommentController : AcademyControllerBase
    {
        private readonly IHotCommentApiService _hotCommentApiSvc;
        private readonly IScheduleService _scheduleService;
        private readonly RedisUtility _redisUtility;
        private readonly ICapPublisher _capPublisher;

        public CommentController(IHotCommentApiService hotCommentApiSvc,
        IScheduleService scheduleService,
        RedisUtility redisUtility,
        ICapPublisher capPublisher)
        {
            _hotCommentApiSvc = hotCommentApiSvc;
            _scheduleService = scheduleService;
            _redisUtility = redisUtility;
            _capPublisher = capPublisher;
        }

        [HttpPost]
        public async Task<Guid> Post([FromBody]CommentInput input)
        {
            input.UserId = UserId;
            input.Ip = HttpContext.GetUserIp();
            return await _hotCommentApiSvc.PublishAsync(input);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IEnumerable<CommentItem>> Get(string objectId, Guid? parentId)
        {
            var comments = await _hotCommentApiSvc.ListAsync(objectId, parentId);

            var uid = IsAuthenticated ? UserId : Guid.Empty;

            return
            comments.Select(c => new Model.CommentItemDto
            {
                Deletable = c.UserId == uid,
                Replyable = c.UserId != uid,
                Content = c.Content,
                DateAdded = c.DateAdded,
                Id = c.Id,
                UserName = c.UserName,
                HomeUrl = c.HomeUrl
            });
        }

        [HttpDelete("{id:guid}")]
        public async Task Delete(Guid id)
        {
            await _hotCommentApiSvc.DeleteAsync(id, UserId);
        }

        [CapSubscribe("Academy.ItemDoneRecord.CommentPublished")]
        public async Task OnCommentPublished([FromBody]CommentPublishedEvent e)
        {
            //幂等性：redis去重
            var key = "academy.itemdonerecord.comment" + e.Id.ToString();
            if (await _redisUtility.DistinctAsync(key))
            {
                //并发控制
                await _scheduleService.IncreaseCommentCountAsync(e.ObjectId);

                //通知
                var item = await _scheduleService.ItemDoneRecordOf(e.ObjectId);
                if (item == null) return;
                var comment = await _hotCommentApiSvc.FetchCommentAsync(e.Id);
                if (comment == null) return;

                //自己的评论自己的打卡记录，并且不是回复别人的，不通知
                if (item.UserId == e.UserId && !comment.ParentId.HasValue) return;

                string content = "";
                int recipientId = 0;
                if (!comment.ParentId.HasValue)
                {
                    var sb = new StringBuilder();
                    sb.Append($"{item.Name},您好:<br/><br/>");
                    sb.Append($"<a href='{comment.HomeUrl}'>{comment.UserName}</a>");
                    sb.Append($"在<a href='{item.Url}' target='_blank'>{WebUtility.HtmlEncode(item.Title)}</a> - 打卡记录中给您评论:<br/><br/>");
                    sb.Append($"<div style='padding:10px;background:#EEE;'>{comment.Content}</div><br/>");
                    sb.Append($"快去看看吧：<a href='{item.Url + "#" + comment.Id}' target='_blank'>{item.Url}</a>");
                    sb.Append($"<br/><br/><hr><div style='color:#666'>博客园：<a style='color:#666' href='http://www.cnblogs.com'>http://www.cnblogs.com</a><br/></div>");
                    content = sb.ToString();
                    recipientId = item.SpaceUserId;
                }
                else
                {
                    var pComment = await _hotCommentApiSvc.FetchCommentAsync(comment.ParentId.Value);
                    //自己回复自己的评论，不通知
                    if (pComment == null || pComment.UserId == e.UserId) return;
                    var sb = new StringBuilder();
                    sb.Append($"{pComment.UserName},您好:<br/><br/>");
                    sb.Append($"<a href='{comment.HomeUrl}'>{comment.UserName}</a>");
                    sb.Append($"回复了您的评论:<br/><br/>");
                    sb.Append($"<div style='padding:10px;background:#EEE;'>{comment.Content}</div><br/>");
                    sb.Append($"快去看看吧：<a href='{item.Url + "#" + comment.Id}' target='_blank'>{item.Url}</a>");
                    sb.Append($"<br/><br/><hr><div style='color:#666'>博客园：<a style='color:#666' href='http://www.cnblogs.com'>http://www.cnblogs.com</a><br/></div>");
                    content = sb.ToString();
                    recipientId = pComment.SpaceuserId;
                }

                var notification = new Model.NotificationEvent()
                {
                    Title = "[打卡记录评论通知]" + item.Title,
                    Content = content,
                    RecipientId = recipientId,
                    Ip = comment.Ip
                };
                await _capPublisher.PublishAsync("Msg.Notification", notification);
            }
        }

        [CapSubscribe("Academy.ItemDoneRecord.CommentDeleted")]
        public async Task OnCommentDeleted([FromBody]CommentDeletedEvent e)
        {
            var key = "-academy.itemdonerecord.comment" + e.Id.ToString();
            if (await _redisUtility.DistinctAsync(key))
            {
                await _scheduleService.IncreaseCommentCountAsync(e.ObjectId, -1);
            }
        }
    }
}
