using System;
using MediatR;

namespace Cnblogs.Academy.Commands.ItemCommands
{
    public class DeleteItemCommand : IRequest<bool>
    {
        public DeleteItemCommand(long id, Guid userId)
        {
            Id = id;
            UserId = userId;
        }

        public long Id { get; }
        public Guid UserId { get; }
    }
}
