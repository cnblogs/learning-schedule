using System;
using MediatR;

namespace Cnblogs.Academy.Commands.ItemCommands
{
    public class UpdateItemTitleCommand : IRequest<bool>
    {

        public UpdateItemTitleCommand(long id, string title, Guid userId)
        {
            Id = id;
            Title = title;
            UserId = userId;
        }

        public long Id { get; }
        public string Title { get; }
        public Guid UserId { get; }
    }
}
