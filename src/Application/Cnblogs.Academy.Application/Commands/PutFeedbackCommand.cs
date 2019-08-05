using System;
using Cnblogs.Academy.Domain.Schedules;
using MediatR;

namespace Cnblogs.Academy.Application.Commands
{
    public class PutFeedbackCommand : IRequest<long>
    {
        public long Id { get; }
        public long ItemId { get; }
        public string Content { get; }
        public Difficulty Difficulty { get; }
        public Guid UserId { get; }

        public PutFeedbackCommand(long id, long itemId, string content, Difficulty difficulty, Guid userId)
        {
            Id = id;
            ItemId = itemId;
            Content = content;
            Difficulty = difficulty;
            UserId = userId;
        }
    }
}
