using System.Linq.Expressions;
using Tusur.Practices.Application.Domain.Entities;
using Tusur.Practices.Application.Domain.Models.Result;
using Tusur.Practices.Application.Ports.Output;

namespace Tusur.Practices.Application.Ports.Input
{
    public interface IManager<TDomain>
        where TDomain : DomainEntity
    {
        IService<TDomain> Service { get; set; }

        public RequestResult<IEnumerable<T>> GetAll<T>() where T : TDomain
        {
            return ((IService<T>)Service).GetAll();
        }

        public RequestResult<IEnumerable<T>> GetBy<T>(Expression<Func<T, bool>> predicate) where T : TDomain
        {
            return ((IService<T>)Service).GetBy(predicate);
        }

        public RequestResult<T> Find<T>(Guid id) where T : TDomain
        {
            return ((IService<T>)Service).Find(id);
        }

        public RequestResult<T> Find<T>(Expression<Func<T, bool>> predicate) where T : TDomain
        {
            return ((IService<T>)Service).Find(predicate);
        }

        public RequestResult<T> Create<T>(T entity) where T : TDomain
        {
            return ((IService<T>)Service).Create(entity);
        }

        public RequestResult<T> Update<T>(T entity) where T : TDomain
        {
            return ((IService<T>)Service).Update(entity);
        }

        public RequestResult<T> Remove<T>(Guid id) where T : TDomain
        {
            return ((IService<T>)Service).Remove(id);
        }
    }
}
