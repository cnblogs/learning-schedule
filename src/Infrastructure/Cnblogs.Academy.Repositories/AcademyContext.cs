using System.Threading;
using System.Threading.Tasks;
using Cnblogs.Academy.Domain.Categories;
using Cnblogs.Academy.Domain.Schedules;
using Cnblogs.Domain.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Schedule = Cnblogs.Academy.Domain.Schedules.Schedule;
using DotNetCore.CAP;

namespace Cnblogs.Academy.Repositories
{
    public class AcademyContext : DbContext, IUnitOfWork
    {
        const string _getDate = "SYSDATETIMEOFFSET()";

        public AcademyContext(DbContextOptions options) : base(options)
        {
        }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            //https://stackoverflow.com/questions/45804470/the-dbcontext-of-type-cannot-be-pooled-because-it-does-not-have-a-single-public
            var bus = this.GetService<ICapPublisher>();
            using (Database.BeginTransaction(bus, autoCommit: true))
            {
                if (await SaveChangesAsync(cancellationToken) > 0)
                {
                    await bus.DispatchDomianEventAsync(this);
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<ScheduleItem> ScheduleItems { get; set; }
        public DbSet<ItemDoneRecord> Records { get; set; }
        public DbSet<ScheduleFollowing> Following { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SchedulePrivateUpdateRecord> SchedulePrivateUpdateRecord { get; set; }
        public DbSet<Subtask> Subtasks { get; set; }
        public DbSet<Reference> References { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Schedule>(
                entity =>
                {
                    entity.Property(s => s.DateAdded).HasDefaultValueSql(_getDate);
                    entity.Property(s => s.DateUpdated).HasDefaultValueSql(_getDate);
                    entity.HasQueryFilter(s => !s.Deleted);
                });

            modelBuilder.Entity<ScheduleItem>(
                entity =>
                {
                    entity.HasOne(i => i.Schedule).WithMany(s => s.Items).HasForeignKey(i => i.ScheduleId);
                    entity.Property(i => i.DateAdded).HasDefaultValueSql(_getDate);
                    entity.Property(i => i.DateStart).HasDefaultValueSql(_getDate);
                    entity.HasQueryFilter(x => !x.Deleted);
                    entity.HasOne(i => i.Html).WithOne(h => h.Item)
                        .HasForeignKey<ScheduleItemHtml>(h => h.ScheduleItemId);
                });

            modelBuilder.Entity<ItemDoneRecord>(
                entity =>
                {
                    entity.HasOne(i => i.Item).WithMany(i => i.Records).HasForeignKey(i => i.ItemId);
                    entity.Property(i => i.DoneTime).HasDefaultValueSql(_getDate);
                    entity.HasQueryFilter(i => !i.Deleted);
                });

            modelBuilder.Entity<Subtask>(
                entity =>
                {
                    entity.HasOne(x => x.Item).WithMany(x => x.Subtasks).HasForeignKey(x => x.ItemId);
                    entity.Property(x => x.DateAdded).HasDefaultValueSql(_getDate);
                    entity.HasQueryFilter(x => !x.Deleted);
                });

            modelBuilder.Entity<Reference>(
                entity =>
                {
                    entity.Property(x => x.DateAdded).HasDefaultValueSql(_getDate);
                    entity.HasOne(x => x.Item).WithMany(x => x.References).HasForeignKey(x => x.ItemId);
                    entity.HasQueryFilter(x => !x.Deleted);
                });

            modelBuilder.Entity<Feedback>(
                entity =>
                {
                    entity.Property(x => x.DateAdded).HasDefaultValueSql(_getDate);
                    entity.HasOne(x => x.Item).WithMany(x => x.Feedbacks).HasForeignKey(x => x.ItemId);
                    entity.HasQueryFilter(x => !x.Deleted);
                });

            modelBuilder.Entity<ScheduleFollowing>(
                entity =>
                {
                    entity.ToTable("ScheduleFollowing");
                    entity.HasAlternateKey(f => new { f.ScheduleId, f.UserId });
                    entity.HasOne(f => f.Schedule).WithMany(s => s.Following).HasForeignKey(f => f.ScheduleId);
                    entity.Property(f => f.DateAdded).HasDefaultValueSql(_getDate);
                    entity.HasQueryFilter(f => !f.Deleted);
                });

            modelBuilder.Entity<SchedulePrivateUpdateRecord>(
                entity =>
                {
                    entity.ToTable("SchedulePrivateUpdateRecord");
                    entity.HasOne(x => x.Schedule).WithMany(x => x.PrivateUpdateRecord)
                        .HasForeignKey(x => x.ScheduleId);
                    entity.Property(x => x.DateAdded).HasDefaultValueSql(_getDate);
                    entity.HasIndex(x => x.DateAdded);
                });

            modelBuilder.Entity<Category>(
                entity =>
                {
                    entity.Property(x => x.DateAdded).HasDefaultValueSql(_getDate);
                    entity.HasQueryFilter(x => !x.Deleted);
                });

        }
    }
}
