using Cnblogs.Academy.Common;
using Cnblogs.Academy.DTO;
using MediatR;

namespace Cnblogs.Academy.Application.Commands
{
    public class ToDoItemCommand : IRequest<BooleanResult>
    {
        public long ScheduleId { get; set; }
        public long ItemId { get; set; }
        public UserDto User { get; set; }

        public ToDoItemCommand(long scheduleId, long itemId, UserDto user)
        {
            ScheduleId = scheduleId;
            ItemId = itemId;
            User = user;
        }
    }
}
