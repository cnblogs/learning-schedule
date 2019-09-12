using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Cnblogs.Domain.Abstract
{
    public abstract class Repository<TDbContext, T> : IRepository<T>
    where T : IAggregateRoot
    where TDbContext : DbContext, IUnitOfWork
    {
        protected readonly TDbContext _context;
        public Repository(TDbContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<IDbTransaction> BeginTransactionAsync(CancellationToken token = default(CancellationToken))
        {
            var dbContextTransaction = await _context.Database.BeginTransactionAsync(token);
            return dbContextTransaction.GetDbTransaction();
        }
    }
}
