using System.Linq;
using System.Threading.Tasks;
using Cnblogs.Academy.Domain;
using Cnblogs.Academy.Domain.Events;
using Cnblogs.Academy.Domain.Schedules;
using Cnblogs.Academy.ServiceAgent.MsgApi;
using Cnblogs.Academy.ServiceAgent.UCenterService;
using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;

namespace Cnblogs.Academy.Subscriber
{
    public class FeedbackEventHandler : IFeedbackEventHandler, ICapSubscribe
    {
        private readonly IUCenterService _uCenterSvc;
        private readonly IScheduleRepository _repository;
        private readonly IMsgApiService _msgApi;

        public FeedbackEventHandler(IUCenterService uCenterSvc, IScheduleRepository repository, IMsgApiService msgApi)
        {
            _uCenterSvc = uCenterSvc;
            _repository = repository;
            _msgApi = msgApi;
        }

        [CapSubscribe("ScheduleItem.Feedback.Created")]
        public async Task HandleFeedbackCreatedEvent(FeedbackCreatedEvent e)
        {
            var item = await _repository.ScheduleItems.FirstOrDefaultAsync(x => x.Id == e.Feedback.ItemId);
            var studentId = item.UserId;
            var teacherId = e.Feedback.UserId;
            if (studentId == teacherId) return;

            var users = await _uCenterSvc.GetUsersByUserIds(new[] { studentId, teacherId });
            var student = users.FirstOrDefault(x => x.UserId == studentId);
            var teacher = users.FirstOrDefault(x => x.UserId == teacherId);

            var notification = new Notification
            {
                Title = "您制定的学习任务有新反馈了",
                RecipientId = teacher.SpaceUserId,
                Content = $@"<p>亲爱的 {teacher.DisplayName} 同学，</p>
                <p>{student.DisplayName} 认为您制定的学习任务<b>{e.Feedback.Difficulty.ToHumanString()}</b></p>
                <h5><a href='{AppConst.DomainAddress}/schedules/u/{teacher.Alias}/{item.ScheduleId}/item/{item.Id}/details'>{item.Title}</a></h5>"
            };
            await _msgApi.NotifyAsync(notification);
        }
    }
}
