using System;
using Cnblogs.Academy.Application.ScheduleAppService;
using Cnblogs.UCenter.DTO.Users;
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
