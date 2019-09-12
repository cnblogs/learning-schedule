using System;
using MediatR;

namespace Cnblogs.Academy.Application.ItemCommands
{
    public class DeleteSummaryLinkCommand:IRequest<bool>
    {
		public long ItemId { get; }
		public long LinkId { get; }
		public Guid UserId { get; }

		public DeleteSummaryLinkCommand(long itemId, long linkId, Guid userId)
        {
			UserId = userId;
			LinkId = linkId;
			ItemId = itemId;
		}
    }
}
