using System;
using Cnblogs.Academy.Application.ScheduleAppService;
using MediatR;

namespace Cnblogs.Academy.Commands.ScheduleCommands
{
    public class UpdateScheduleCommand : IRequest<bool>
    {
        public UpdateScheduleCommand(long id, ScheduleInputModel input, Guid userId)
        {
            Id = id;
            Model = input;
            UserId = userId;
        }

        public long Id { get; }
        public ScheduleInputModel Model { get; }
        public Guid UserId { get; }
    }
}
