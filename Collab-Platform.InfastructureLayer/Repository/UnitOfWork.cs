using Collab_Platform.ApplicationLayer.Interface.RepoInterface;
using Collab_Platform.InfastructureLayer.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Collab_Platform.InfastructureLayer.Repository
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        private IDbContextTransaction _transaction;
        public UnitOfWork(ApplicationDbContext db) { 
            _db = db;
        }

        public async Task BeginTranctionAsync()
        {
            _transaction = await _db.Database.BeginTransactionAsync();
        }

        public async Task CommitTranctionAsync()
        {
            if (_transaction == null)
                throw new InvalidOperationException("Transaction has not been started.");
            await _transaction.CommitAsync();
        }

        public async Task RollBackTranctionAsync()
        {
            if (_transaction == null)
                throw new InvalidOperationException("Transaction has not been started");
            await _transaction.RollbackAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _db.SaveChangesAsync();
        }
    }
}
