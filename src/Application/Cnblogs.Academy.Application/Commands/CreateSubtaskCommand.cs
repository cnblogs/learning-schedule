using System;
using MediatR;

namespace Cnblogs.Academy.Application.Commands
{
    public class CreateSubtaskCommand : IRequest<long>
    {
        public CreateSubtaskCommand(long itemId, string content, Guid userId)
        {
            ItemId = itemId;
            Content = content;
            UserId = userId;
        }

        public long ItemId { get; set; }
        public string Content { get; set; }
        public Guid UserId { get; set; }
    }
}
