using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Tusur.Practices.Persistence.Database;
using Tusur.Practices.Persistence.Database.Entities.Utils;
using Tusur.Practices.Persistence.Exceptions;

namespace Tusur.Practices.Persistence.Repositories.Implementations
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        protected readonly ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _context.Set<T>().AsNoTracking().ToList();
        }

        public virtual T Find(Guid id)
        {
            return _context.Set<T>().AsNoTracking().FirstOrDefault(entity => entity.Id == id)
                ?? throw new EntityNotFoundException();
        }

        public virtual async Task<T> FindAsync(Guid id)
        {
            return await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(entity => entity.Id == id)
                ?? throw new EntityNotFoundException();
        }

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().AsNoTracking().Where(predicate).ToList();
        }

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().AsNoTracking().Where(predicate).ToListAsync();
        }

        public virtual T Create(T entity)
        {
            var result = _context.Add(entity);
            return result.Entity;
        }

        public virtual T Update(T entity)
        {
            var result = _context.Update(entity);
            return result.Entity;
        }

        public virtual T Remove(T entity)
        {
            var result = _context.Remove(entity);
            return result.Entity;
        }

        public virtual void SaveChanges()
        {
            _context.SaveChanges();
        }

        public virtual async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            
        }
    }
}
