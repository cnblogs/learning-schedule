using System;
using Cnblogs.UCenter.DTO.Users;
using MediatR;

namespace Cnblogs.Academy.Application.Commands
{
    public class CompleteItemCommand : IRequest<BooleanResult>
    {
        public long ScheduleId { get; set; }
        public long ItemId { get; set; }
        public Guid UserId { get; set; }

        public CompleteItemCommand(long scheduleId, long itemId, Guid userId)
        {
            ScheduleId = scheduleId;
            ItemId = itemId;
            UserId = userId;
        }
    }
}
