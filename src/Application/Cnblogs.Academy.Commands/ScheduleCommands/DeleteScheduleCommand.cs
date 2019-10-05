using System;
using MediatR;

namespace Cnblogs.Academy.Commands.ScheduleCommands
{
    public class DeleteScheduleCommand : IRequest<bool>
    {
        public DeleteScheduleCommand(long id, Guid userId)
        {
            Id = id;
            UserId = userId;
        }

        public long Id { get; }
        public Guid UserId { get; }
    }
}
