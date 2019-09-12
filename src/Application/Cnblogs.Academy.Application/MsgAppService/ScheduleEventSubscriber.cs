using System.Linq;
using System.Threading.Tasks;
using Cnblogs.Academy.Domain;
using Cnblogs.Academy.Domain.Events;
using Cnblogs.Academy.Domain.Schedules;
using Cnblogs.Academy.Domain.Schedules.Events;
using Cnblogs.Academy.ServiceAgent.MsgApi;
using Cnblogs.UCenter.ServiceAgent;
using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;

namespace Cnblogs.Academy.Application.MsgAppService
{
    public class ScheduleEventSubscriber : IScheduleEventSubscriber, ICapSubscribe
    {
        private readonly IMsgApiService _msgSvc;
        private readonly IUCenterService _uCenter;
        private readonly IScheduleRepository _repository;

        public ScheduleEventSubscriber(IMsgApiService msgSvc, IUCenterService uCenter, IScheduleRepository repository)
        {
            _msgSvc = msgSvc;
            _uCenter = uCenter;
            _repository = repository;
        }

        [CapSubscribe(EventConst.NewSubscriber, Group = MsgAppConst.MessageGroup)]
        public async Task HandleNewSubscriberEvent(NewSubscriberEvent e)
        {
            await Task.CompletedTask;
            // Notify schedule auth
            // var child = await _repository.FindByUUID<Schedule>(e.ChildScheduleUuid).Include(x => x.Parent).FirstOrDefaultAsync();
            // if (child == null) return;

            // var auth = await _uCenter.GetUser(x => x.UserId, child.Parent.UserId);
            // if (auth == null) return;

            // var subscriber = await _uCenter.GetUser(x => x.UserId, child.UserId);
            // if (subscriber == null) return;

            // var msg = new Notification
            // {
            //     RecipientId = auth.SpaceUserId,
            //     Title = $"有用户借鉴了您的「{child.Parent.Title}」",
            //     Content = $@"<p>亲爱的 {auth.DisplayName} 同学：</p>
            //             <p>
            //                 <b>
            //                 <a href='{AppConst.DomainAddress}/schedules/u/{subscriber.Alias}/{child.Id}'>
            //                     {subscriber.DisplayName}</a>
            //                 </b>
            //                 借鉴了您的学习计划
            //                 <a href='{AppConst.DomainAddress}/schedules/u/{auth.Alias}/{child.ParentId}'>
            //                 「{child.Parent.Title}」
            //                 </a>
            //             </p>"
            // };
            // await _msgSvc.NotifyAsync(msg);
        }

        [CapSubscribe(EventConst.ScheduleCompletedEvent, Group = MsgAppConst.MessageGroup)]
        public async Task HandleScheduleCompletedEvent(ScheduleCompletedEvent e)
        {
            await Task.CompletedTask;
            // var schedule = await _repository.FindByUUID<Schedule>(e.ScheduleUuid).FirstOrDefaultAsync();
            // if (schedule == null) return;

            // if (schedule.ParentId > 0)
            // {
            //     var parent = await _repository.Schedules.Where(x => x.Id == schedule.ParentId).FirstOrDefaultAsync();
            //     if (parent == null) return;

            //     var user = await _uCenter.GetUser(x => x.UserId, schedule.UserId);
            //     if (user == null) return;

            //     var recipient = await _uCenter.GetUser(x => x.UserId, parent.UserId);
            //     if (recipient == null) return;

            //     await _msgSvc.NotifyAsync(new Notification
            //     {
            //         RecipientId = recipient.SpaceUserId,
            //         Title = $"有同学完成了您制定的学习计划",
            //         Content = $@"<p>亲爱的 {recipient.DisplayName} 同学：</p>
            //             <p><a href='{AppConst.DomainAddress}/schedules/u/{user.Alias}/{schedule.Id}'>
            //                 {user.DisplayName}</a>完成了您制定的学习计划: </p>
            //             <p>
            //             <a href='{AppConst.DomainAddress}/schedules/u/{user.Alias}/{schedule.Id}'>
            //                     <b>{parent.Title}</b>
            //             </a>
            //             </p>"
            //     });
            // }
        }

        [CapSubscribe(EventConst.ChildScheduleUpdatedEvent, Group = MsgAppConst.MessageGroup)]
        public async Task HandleChildScheduleUpdatedEvent(ChildScheduleUpdatedEvent e)
        {
            var child = await _repository.FindByUUID<Schedule>(e.ScheduleUuid).FirstOrDefaultAsync();
            if (child == null) return;

            var recipient = await _uCenter.GetUser(x => x.UserId, child.UserId);
            if (recipient == null) return;

            var msg = new Notification
            {
                RecipientId = recipient.SpaceUserId,
                Title = $"您借鉴的学习计划「{e.LegacyTitle}」改名了",
                Content = $@"<p>亲爱的 {recipient.DisplayName} 同学：</p>
                        <p>您借鉴的学习计划「{e.LegacyTitle}」改名了</p>
                        <h5>
                            <del>
                                <b>{e.LegacyTitle}</b>
                            </del>
                        </h5>
                        <h5>
                            <a href='{AppConst.DomainAddress}/schedules/u/{recipient.Alias}/{child.Id}'>
                                <b>{child.Title}</b>
                            </a>
                        </h5>"
            };
            await _msgSvc.NotifyAsync(msg);
        }
    }
}
