using Cnblogs.Academy.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cnblogs.Academy.Repositories.EntityConfigurations
{
    public abstract class BaseEntityTypeConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<T> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.UUID).HasDefaultValueSql("newsequentialid()");
            entity.HasIndex(x => x.UUID).IsUnique();
            entity.Ignore(x => x.DomainEvents);
        }
    }

    public abstract class EntityTypeConfiguration<T> : BaseEntityTypeConfiguration<T> where T : Entity
    {
        public override void Configure(EntityTypeBuilder<T> entity)
        {
            base.Configure(entity);
            entity.Property(x => x.DateAdded).HasDefaultValueSql(EntityConst.GetDate);
            entity.HasQueryFilter(s => !s.Deleted);
        }
    }
}
