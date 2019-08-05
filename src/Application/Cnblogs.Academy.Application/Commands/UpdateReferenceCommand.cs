using System;
using MediatR;

namespace Cnblogs.Academy.Application.Commands
{
    public class UpdateReferenceCommand : IRequest<bool>
    {
        public long ItemId { get; set; }
        public long RefId { get; set; }
        public string Url { get; set; }
        public Guid UserId { get; set; }

        public UpdateReferenceCommand(long itemId, long refId, string url, Guid userId)
        {
            UserId = userId;
            Url = url;
            RefId = refId;
            ItemId = itemId;
        }
    }
}
