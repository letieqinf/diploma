using Tusur.Practices.Persistence.Database;
using Tusur.Practices.Persistence.Database.Entities.Utils;
using Tusur.Practices.Persistence.Repositories.Implementations;

namespace Tusur.Practices.Persistence.UnitsOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly Dictionary<Type, object> _repositories;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            _repositories = new Dictionary<Type, object>();
        }

        public Repository<TEntity> GetRepository<TEntity>()
            where TEntity : Entity
        {
            var type = typeof(TEntity);

            if (!_repositories.ContainsKey(type))
            {
                var repository = Activator.CreateInstance(typeof(Repository<TEntity>), new object[] { _context });
                _repositories.Add(type, repository!);
            }

            return (Repository<TEntity>)_repositories[type];
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
