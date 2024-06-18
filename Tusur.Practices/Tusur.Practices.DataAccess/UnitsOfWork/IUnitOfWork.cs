using Tusur.Practices.Persistence.Database.Entities.Utils;
using Tusur.Practices.Persistence.Repositories;
using Tusur.Practices.Persistence.Repositories.Implementations;

namespace Tusur.Practices.Persistence.UnitsOfWork
{
    public interface IUnitOfWork
    {
        public Repository<TEntity> GetRepository<TEntity>() where TEntity : Entity;
        public void SaveChanges();
        public Task SaveChangesAsync();
    }
}
