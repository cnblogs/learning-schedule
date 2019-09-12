using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cnblogs.Academy.Domain.Schedules;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cnblogs.Academy.Application.Commands
{
    public class UndoItemCommandHandler : IRequestHandler<UndoItemCommand, BooleanResult>
    {
        private readonly IScheduleRepository _repository;

        public UndoItemCommandHandler(IScheduleRepository repository)
        {
            _repository = repository;
        }

        public async Task<BooleanResult> Handle(UndoItemCommand request, CancellationToken cancellationToken)
        {
            var item = await _repository.ScheduleItems
                                        .Where(x => x.Id == request.ItemId && x.ScheduleId == request.ScheduleId && x.Schedule.UserId == request.UserId)
                                        .FirstOrDefaultAsync() ?? throw new ValidationException("无效的取消打卡请求");

            item.Undo();

            await _repository.UnitOfWork.SaveEntitiesAsync();

            return BooleanResult.Succeed();
        }
    }
}
