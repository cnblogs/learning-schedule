using System;
using Cnblogs.Academy.DTO;
using MediatR;

namespace Cnblogs.Academy.Application.ItemCommands
{
    public class AddSummaryLinkCommand : IRequest<long>
    {
        public AddSummaryLinkCommand(long itemId, Guid userId, SummaryLinkDto linkDto)
        {
            ItemId = itemId;
            UserId = userId;
            LinkDto = linkDto;
        }

        public long ItemId { get; }
        public Guid UserId { get; }
        public SummaryLinkDto LinkDto { get; }
    }
}
