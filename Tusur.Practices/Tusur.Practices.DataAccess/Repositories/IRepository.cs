using System.Linq.Expressions;
using Tusur.Practices.Persistence.Database.Entities.Utils;

namespace Tusur.Practices.Persistence.Repositories
{
    public interface IRepository<T> : IDisposable
        where T : IEntity
    {
        public IEnumerable<T> GetAll();

        public T Find(Guid id);
        public Task<T> FindAsync(Guid id);
        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        public Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        public T Create(T entity);
        public T Update(T entity);
        public T Remove(T entity);

        public void SaveChanges();
        public Task SaveChangesAsync();
    }
}
