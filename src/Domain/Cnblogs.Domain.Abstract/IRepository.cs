using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Cnblogs.Domain.Abstract
{
    public interface IRepository<T> where T : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
        
        Task<IDbTransaction> BeginTransactionAsync(CancellationToken token = default(CancellationToken));
    }
}
