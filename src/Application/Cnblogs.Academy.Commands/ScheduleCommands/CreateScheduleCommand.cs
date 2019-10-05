using System;
using Cnblogs.Academy.Application.ScheduleAppService;
using Cnblogs.Academy.Common;
using Cnblogs.Academy.DTO;
using MediatR;

namespace Cnblogs.Academy.Commands.ScheduleCommands
{
    public class CreateScheduleCommand : IRequest<BooleanResult>
    {

        public CreateScheduleCommand(ScheduleInputModel model, UserDto user)
        {
            Model = model;
            User = user;
        }

        public ScheduleInputModel Model { get; }
        public UserDto User { get; }
    }
}
