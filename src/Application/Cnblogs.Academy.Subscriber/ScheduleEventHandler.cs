using System.Threading.Tasks;
using Cnblogs.Academy.Domain;
using Cnblogs.Academy.Events;
using Cnblogs.Academy.ServiceAgent.MsgApi;
using Cnblogs.Academy.ServiceAgent.UCenterService;
using DotNetCore.CAP;

namespace Cnblogs.Academy.Subscriber
{
    public class ScheduleEventHandler : IScheduleEventHandler, ICapSubscribe
    {
        private readonly IMsgApiService _msgApi;
        private readonly IUCenterService _uCenterService;

        public ScheduleEventHandler(IMsgApiService msgApi, IUCenterService uCenterService)
        {
            _msgApi = msgApi;
            _uCenterService = uCenterService;
        }

        [CapSubscribe("ScheduleItem.Created")]
        public async Task HandleScheduleItemCreated(ScheduleItemCreatedEvent e)
        {
            if (e.ScheduleUserId == e.UserId)
            {
                return; //自己制定的不用通知
            }
            var student = await _uCenterService.GetUser(x => x.UserId, e.ScheduleUserId);
            var teacher = await _uCenterService.GetUser(x => x.UserId, e.UserId);
            var notification = new Notification
            {
                Title = "有人帮你制定了学习任务",
                RecipientId = student.SpaceUserId,
                Content = $@"<p>亲爱的 {student.DisplayName} 同学，<p>
                <p><a href='{AppConst.DomainAddress}/u/{teacher.Alias}'>{teacher.DisplayName}</a>
                帮你制定了学习任务：
                <a href='{e.Link}'>{e.Title}</a></p>"
            };
            await _msgApi.NotifyAsync(notification);
        }

        [CapSubscribe("ScheduleItem.Completed")]
        public async Task HandleScheduleItemCompleted(ScheduleItemCompletedEvent e)
        {
            if (e.UserId == e.ItemUserId)
            {
                return; // 自己完成了自己的任务，不用通知
            }
            var student = await _uCenterService.GetUser(x => x.UserId, e.UserId);
            var teacher = await _uCenterService.GetUser(x => x.UserId, e.ItemUserId);
            var notification = new Notification
            {
                Title = "已完成您制定的学习任务",
                RecipientId = teacher.SpaceUserId,
                Content = $@"<p>亲爱的 {teacher.DisplayName} 同学,</p>
                <p><a href='{AppConst.DomainAddress}/u/{student.Alias}'>{student.DisplayName}</a>
                完成了您制定的学习任务:
                <a href='{e.Link}'>{e.Title}</a></p>"
            };
            await _msgApi.NotifyAsync(notification);
        }
    }
}
