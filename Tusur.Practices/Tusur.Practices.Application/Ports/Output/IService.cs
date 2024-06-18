using System.Linq.Expressions;
using Tusur.Practices.Application.Domain.Entities;
using Tusur.Practices.Application.Domain.Models.Result;

namespace Tusur.Practices.Application.Ports.Output
{
    public interface IService<T>
        where T : DomainEntity
    {
        public RequestResult<IEnumerable<T>> GetAll();
        public RequestResult<IEnumerable<T>> GetBy(Expression<Func<T, bool>> predicate);
        public RequestResult<T> Find(Guid id);
        public RequestResult<T> Find(Expression<Func<T, bool>> predicate);
        public RequestResult<T> Create(T entity);
        public RequestResult<T> Update(T entity);
        public RequestResult<T> Remove(Guid id);
    }
}
