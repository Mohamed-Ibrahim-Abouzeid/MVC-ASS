using GymManagementSystemG01.DAL.Repositories.Interfaces;
using GYMSystem.DAL.DBContexts;
using GYMSystem.DAL.Models;

namespace GymManagementSystemG01.DAL.Repositories.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GYMDBContext _dbContext;

        public UnitOfWork(GYMDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IGenericRepository<TEntity> GetRepository<TEntity>()
            where TEntity : BaseEntity, new()
        {
            return new GenericRepository<TEntity>(_dbContext);
        }

        public Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            return _dbContext.SaveChangesAsync(ct);
        }

        public ISessionRepository SessionRepository
        {
            get
            {
                return new SessionRepository(_dbContext);
            }
        }
    }
}