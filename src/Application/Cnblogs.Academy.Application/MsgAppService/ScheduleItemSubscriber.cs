using System.Linq;
using System.Threading.Tasks;
using Cnblogs.Academy.Domain;
using Cnblogs.Academy.Domain.Events;
using Cnblogs.Academy.Domain.Schedules;
using Cnblogs.Academy.Domain.Schedules.Events;
using Cnblogs.Academy.ServiceAgent.MsgApi;
using Cnblogs.Academy.ServiceAgent.UCenterService;
using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;

namespace Cnblogs.Academy.Application.MsgAppService
{
    public class ScheduleItemSubscriber : IScheduleItemSubscriber, ICapSubscribe
    {
        private readonly IMsgApiService _msgSvc;
        private readonly IUCenterService _uCenter;
        private readonly IScheduleRepository _repository;

        public ScheduleItemSubscriber(IMsgApiService msgSvc, IUCenterService uCenter, IScheduleRepository repository)
        {
            _msgSvc = msgSvc;
            _uCenter = uCenter;
            _repository = repository;
        }

        [CapSubscribe(EventConst.ScheduleItemCompletedEvent, Group = MsgAppConst.MessageGroup)]
        public async Task HandleScheduleItemCompletedEvent(ScheduleItemCompletedEvent e)
        {
            await Task.CompletedTask;
            // var item = await _repository.FindByUUID<ScheduleItem>(e.ItemUuid).FirstOrDefaultAsync();
            // if (item == null) return;

            // if (item.ParentId > 0)
            // {
            //     var parent = await _repository.ScheduleItems.AsNoTracking()
            //         .Include(x => x.Schedule)
            //         .Include(x => x.Html)
            //         .Where(x => x.Id == item.ParentId)
            //         .FirstOrDefaultAsync();
            //     if (parent == null) return;

            //     var recipient = await _uCenter.GetUser(x => x.UserId, parent.UserId);

            //     var subscriber = await _uCenter.GetUser(x => x.UserId, item.UserId);
            //     if (subscriber == null) return;

            //     await _msgSvc.NotifyAsync(new Notification
            //     {
            //         RecipientId = recipient.SpaceUserId,
            //         Title = $"有同学完成了您的制定的学习任务",
            //         Content = $@"<p>亲爱的 {recipient.DisplayName} 同学，</p>
            //             <p><a href='{AppConst.DomainAddress}/schedules/u/{subscriber.Alias}/{item.ScheduleId}'>
            //                 {subscriber.DisplayName}</a>完成了您制定的学习任务: </p>
            //             <p>
            //             <a href='{AppConst.DomainAddress}/schedules/u/{subscriber.Alias}/{item.ScheduleId}'>
            //                     <b>{parent.Schedule.Title}:{parent.GenerateDescription()}</b>
            //             </a>
            //             </p>"
            //     });
            // }
        }

        [CapSubscribe(EventConst.ChildScheduleUpdatedEvent, Group = MsgAppConst.MessageGroup)]
        public async Task HandleChildScheduleUpdatedEvent(ScheduleItemDeletedEvent e)
        {
            var child = await _repository.FindByUUID<ScheduleItem>(e.ItemUuid)
                .Include(x => x.Schedule)
                .Include(x => x.Html)
                .FirstOrDefaultAsync();

            if (child == null) return;

            var recipient = await _uCenter.GetUser(x => x.UserId, child.Schedule.UserId);
            if (recipient == null) return;

            var msg = new Notification
            {
                RecipientId = recipient.SpaceUserId,
                Title = $"您借鉴的学习计划「{child.Schedule.Title}」中有学习任务被删除了",
                Content = $@"<p>亲爱的 {recipient.DisplayName} 同学：</p>
                        <p>您借鉴的学习计划「{child.Schedule.Title}」中有学习任务被删除了</p>
                        <h5>
                            <del>
                                <b>{child.GenerateDescription()}</b>
                            </del>
                        </h5>
                        <h5>
                            <span>学习计划: </span>
                            <a href='{AppConst.DomainAddress}/schedules/u/{recipient.Alias}/{child.ScheduleId}'>
                                <b>{child.Schedule.Title}</b>
                            </a>
                        </h5>"
            };
            await _msgSvc.NotifyAsync(msg);
        }

        [CapSubscribe(EventConst.ScheduleItemDispatchedEvent, Group = MsgAppConst.MessageGroup)]
        public async Task HandleScheduleItemDispatchedEvent(ScheduleItemDispatchedEvent e)
        {
            var item = await _repository.FindByUUID<ScheduleItem>(e.ChildItemUuid)
                .Include(x => x.Schedule)
                .Include(x => x.Html)
                .FirstOrDefaultAsync();

            if (item == null) return;

            var user = await _uCenter.GetUser(x => x.UserId, item.UserId);
            if (user == null) return;

            var msg = new Notification
            {
                Title = $"您借鉴的学习计划「{item.Schedule.Title}」发布了新任务",
                RecipientId = user.SpaceUserId,
                Content = $@"<p>亲爱的 {user.DisplayName} 同学：</p>
                <p>您借鉴的学习计划「{item.Schedule.Title}」发布了新任务，赶紧去学习吧</p>
                <h5>
                    <a href='{AppConst.DomainAddress}/schedules/u/{user.Alias}/{item.ScheduleId}/item/{item.Id}/'>
                        <b>{item.GenerateDescription()}</b>
                    </a>
                </h5>"
            };
            await _msgSvc.NotifyAsync(msg);
        }

        [CapSubscribe(EventConst.ChildScheduleItemUpdatedEvent, Group = MsgAppConst.MessageGroup)]
        public async Task HandleChildScheduleItemUpdatedEvent(ChildScheduleItemUpdatedEvent e)
        {
            var child = await _repository.FindByUUID<ScheduleItem>(e.ItemUuid)
                .Include(x => x.Schedule)
                .Include(x => x.Html)
                .FirstOrDefaultAsync();

            if (child == null) return;

            var recipient = await _uCenter.GetUser(x => x.UserId, child.Schedule.UserId);
            if (recipient == null) return;

            var msg = new Notification
            {
                Title = $"您借鉴的学习计划「{child.Schedule.Title}」更新了学习任务",
                RecipientId = recipient.SpaceUserId,
                Content = $@"<p>亲爱的 {recipient.DisplayName} 同学：</p>
                        <p>您借鉴的学习计划「{child.Schedule.Title}」更新了学习任务</p>
                        <h5>
                            <del>
                                <b>{e.LegacyTitle}</b>
                            </del>
                        </h5>
                        <h5>
                            <a href='{AppConst.DomainAddress}/schedules/u/{recipient.Alias}/{child.ScheduleId}/item/{child.Id}/'>
                                <b>{child.GenerateDescription()}</b>
                            </a>
                        </h5>"
            };
            await _msgSvc.NotifyAsync(msg);
        }
    }
}
