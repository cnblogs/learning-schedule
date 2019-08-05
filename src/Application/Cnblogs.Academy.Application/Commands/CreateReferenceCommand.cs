using System;
using MediatR;

namespace Cnblogs.Academy.Application.Commands
{
    public class CreateReferenceCommand : IRequest<long>
    {
        public CreateReferenceCommand(long itemId, string url, Guid userId)
        {
            ItemId = itemId;
            Url = url;
            UserId = userId;
        }

        public long ItemId { get; set; }
        public string Url { get; set; }
        public Guid UserId { get; set; }
    }
}
