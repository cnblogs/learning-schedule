using Cnblogs.Academy.Application.ScheduleAppService;
using Cnblogs.Academy.DTO;
using MediatR;

namespace Cnblogs.Academy.Commands.ItemCommands
{
    public class AddItemCommand : IRequest<long>
    {
        public AddItemCommand(long scheduleId, ScheduleItemMarkdownInput model, UserDto user)
        {
            ScheduleId = scheduleId;
            Model = model;
            User = user;
        }

        public long ScheduleId { get; }
        public ScheduleItemMarkdownInput Model { get; }
        public UserDto User { get; }
    }
}
