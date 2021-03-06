using System;
using Cnblogs.Academy.Common;
using Cnblogs.Academy.DTO;
using MediatR;

namespace Cnblogs.Academy.Commands.ScheduleCommands
{
    public class CancelScheduleCommand : IRequest<BooleanResult>
    {
        public CancelScheduleCommand(long scheduleId, UserDto user)
        {
            ScheduleId = scheduleId;
            User = user;
        }

        public long ScheduleId { get; }
        public UserDto User { get; }
    }
}
