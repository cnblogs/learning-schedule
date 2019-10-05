using Cnblogs.Academy.Domain.Schedules;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cnblogs.Academy.Repositories.EntityConfigurations
{
    class ScheduleEntityTypeConfiguration : EntityTypeConfiguration<Schedule>
    {
        public override void Configure(EntityTypeBuilder<Schedule> entity)
        {
            base.Configure(entity);

            entity.HasOne(x => x.Parent).WithMany(x => x.Children).HasForeignKey(x => x.ParentId).OnDelete(DeleteBehavior.Restrict);
            entity.HasIndex(nameof(Schedule.UserId), nameof(Schedule.ParentId)).IsUnique()
                          .HasFilter($"[{nameof(Schedule.Deleted)}]=0 and [{nameof(Schedule.ParentId)}] is not null");
            entity.Property(s => s.DateUpdated).HasDefaultValueSql(EntityConst.GetDate);
        }
    }

    class ScheduleItemEntityTypeConfiguration : EntityTypeConfiguration<ScheduleItem>
    {
        public override void Configure(EntityTypeBuilder<ScheduleItem> entity)
        {
            base.Configure(entity);

            entity.HasOne(i => i.Schedule).WithMany(s => s.Items).HasForeignKey(i => i.ScheduleId);
            entity.Property(i => i.DateStart).HasDefaultValueSql(EntityConst.GetDate);
            entity.HasOne(i => i.Html).WithOne(h => h.Item).HasForeignKey<ScheduleItemHtml>(h => h.ScheduleItemId);
            entity.HasOne(x => x.Parent).WithMany(x => x.Children).HasForeignKey(x => x.ParentId).OnDelete(DeleteBehavior.Restrict);
            entity.HasIndex(nameof(ScheduleItem.UserId), nameof(ScheduleItem.ParentId)).IsUnique()
                  .HasFilter($"[{nameof(ScheduleItem.Deleted)}]=0 and [{nameof(ScheduleItem.ParentId)}] is not null");
        }
    }

    class SummaryNoteEntityTypeConfiguration : EntityTypeConfiguration<SummaryNote>
    {
        public override void Configure(EntityTypeBuilder<SummaryNote> entity)
        {
            base.Configure(entity);

            entity.ToTable("SummaryNotes");
            entity.HasOne(x => x.Item).WithOne(x => x.SummaryNote).HasForeignKey<SummaryNote>(x => x.ItemId);
            entity.HasIndex(nameof(SummaryNote.ItemId)).IsUnique().HasFilter($"[{nameof(SummaryNote.Deleted)}] = 0");
        }
    }

    class SummaryLinkEntityTypeConfiguration : EntityTypeConfiguration<SummaryLink>
    {
        public override void Configure(EntityTypeBuilder<SummaryLink> entity)
        {
            base.Configure(entity);

            entity.ToTable("SummaryLinks");
            entity.HasOne(x => x.Item).WithMany(x => x.SummaryLinks).HasForeignKey(x => x.ItemId);
        }
    }
}
