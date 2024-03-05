using System;
using System.Threading.Tasks;
using System.Transactions;
using Ledger.Ledger.Web.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace Ledger.Ledger.Web.UnitOfWork
{

    public interface IUnitOfWork :IDisposable
    {
        Task CommitAsync();
        Task RollBackAsync();
        void BeginTransaction();
        void BeginSerializableTransaction();
        Task SaveChangesAsync();

    }
    
    public class UnitOfWork :IUnitOfWork
    {
        private readonly IDbContext _dbContext;
        private IDbContextTransaction _transaction;

        public UnitOfWork(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public void BeginTransaction()
        {
            _transaction = _dbContext.BeginTransaction();
        }

        public void BeginSerializableTransaction()
        {
            _transaction = _dbContext.BeginSerializableTransaction();
        }
        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
        public async Task CommitAsync()
        {
            await _transaction.CommitAsync();
        }
        public async Task RollBackAsync()
        {
            await _transaction.RollbackAsync();
        }

        public void Dispose()
        {
            _transaction.Dispose();
        }
    }
}