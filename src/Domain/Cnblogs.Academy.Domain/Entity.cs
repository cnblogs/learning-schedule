using System;

namespace Cnblogs.Academy.Domain
{
    public abstract class Entity : BaseEntity
    {
        public DateTimeOffset DateAdded { get; protected set; } = DateTimeOffset.Now;
        public bool Deleted { get; protected set; }

    }
}
