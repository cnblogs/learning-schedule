using System.Threading;
using System.Threading.Tasks;
using Cnblogs.Academy.Domain.Categories;
using Cnblogs.Academy.Domain.Schedules;
using Cnblogs.Domain.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Schedule = Cnblogs.Academy.Domain.Schedules.Schedule;
using DotNetCore.CAP;
using Cnblogs.Academy.Repositories.EntityConfigurations;

namespace Cnblogs.Academy.Repositories
{
    public class AcademyContext : DbContext, IUnitOfWork
    {
        const string _getDate = "SYSDATETIMEOFFSET()";
        public AcademyContext(DbContextOptions options) : base(options) { }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            //https://stackoverflow.com/questions/45804470/the-dbcontext-of-type-cannot-be-pooled-because-it-does-not-have-a-single-public
            var bus = this.GetService<ICapPublisher>();
            using (var trans = Database.BeginTransaction())
            {
                if (await SaveChangesAsync(cancellationToken) > 0)
                {
                    await bus.DispatchDomianEventsAsync(this);
                    trans.Commit();
                }
                else
                {
                    trans.Rollback();
                    return false;
                }
            }
            return true;
        }

        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<ScheduleItem> ScheduleItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SchedulePrivateUpdateRecord> SchedulePrivateUpdateRecord { get; set; }
        public DbSet<SummaryNote> SummaryNotes { get; set; }
        public DbSet<SummaryLink> SummaryLinks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);

            modelBuilder.Entity<SchedulePrivateUpdateRecord>(entity =>
            {
                entity.ToTable("SchedulePrivateUpdateRecord");
                entity.HasOne(x => x.Schedule).WithMany(x => x.PrivateUpdateRecord).HasForeignKey(x => x.ScheduleId);
                entity.Property(x => x.DateAdded).HasDefaultValueSql(EntityConst.GetDate);
                entity.HasIndex(x => x.DateAdded);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(x => x.DateAdded).HasDefaultValueSql(_getDate);
                entity.HasQueryFilter(x => !x.Deleted);
            });
        }
    }
}
