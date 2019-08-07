using System;

namespace Cnblogs.Academy.ServiceAgent.RelationService
{
    public class RelationGroupDto
    {
        public RelationGroupDto()
        {
        }

        public RelationGroupDto(Guid userId, string name)
        {
            UserId = userId;
            Name = name;
        }

        public RelationGroupDto(Guid userId, Guid groupId, string name) : this(userId, name)
        {
            Id = groupId;
        }

        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string Name { get; set; }
    }
}
