using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Cnblogs.Academy.Domain.Schedules;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cnblogs.Academy.Application.Commands
{
    public class PutFeedbackCommandHandler : IRequestHandler<PutFeedbackCommand, long>
    {
        private readonly IScheduleRepository _repository;

        public PutFeedbackCommandHandler(IScheduleRepository repository)
        {
            _repository = repository;
        }

        public async Task<long> Handle(PutFeedbackCommand request, CancellationToken cancellationToken)
        {
            Feedback result;
            if (request.Id < 1)
            {
                var item = await _repository.ScheduleItems.FirstOrDefaultAsync(x => x.Id == request.ItemId && x.Schedule.UserId == request.UserId);
                if (item == null) throw new ValidationException("找不到对应的学习任务");
                result = item.AddFeedback(request.Difficulty, request.Content, request.UserId);
            }
            else
            {
                var feedback = await _repository.Feedbacks.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);
                if (feedback == null) throw new ValidationException("找不到对应的反馈");
                result = feedback.Update(request.Difficulty, request.Content, request.UserId);
            }
            await _repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            return result.Id;
        }
    }
}
