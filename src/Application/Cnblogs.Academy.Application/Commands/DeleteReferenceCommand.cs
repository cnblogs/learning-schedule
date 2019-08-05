using System;
using MediatR;

namespace Cnblogs.Academy.Application.Commands
{
    public class DeleteReferenceCommand : IRequest<bool>
    {
        public long RefId { get; set; }
        public Guid UserId { get; set; }

        public DeleteReferenceCommand(long refId, Guid userId)
        {
            UserId = userId;
            RefId = refId;
        }
    }
}
