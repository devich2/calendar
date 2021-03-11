using System.Threading;
using System.Threading.Tasks;
using Calendar.DAL.Abstract.Transactions;
using Microsoft.EntityFrameworkCore.Storage;

namespace Calendar.DAL.Impl.Transactions
{
    public class DbTransactionWrapper: ITransaction
    {
        private readonly IDbContextTransaction _delegate;

        private bool _finished = false;

        public DbTransactionWrapper(IDbContextTransaction transaction)
        {
            _delegate = transaction;
        }

        public bool Finished => _finished;

        public void Commit()
        {
            _finished = true;
            _delegate.Commit();
        }

        public void Rollback()
        {
            _finished = true;
            _delegate.Rollback();
        }

        public Task CommitAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            _finished = true;
            return _delegate.CommitAsync(cancellationToken);
        }

        public Task RollbackAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            _finished = true;
            return _delegate.RollbackAsync(cancellationToken);
        }
    }
}
