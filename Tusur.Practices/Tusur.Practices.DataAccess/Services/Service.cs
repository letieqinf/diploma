using System.Linq.Expressions;
using Tusur.Practices.Application.Domain.Entities;
using Tusur.Practices.Application.Domain.Models.Result;
using Tusur.Practices.Application.Ports.Output;
using Tusur.Practices.Persistence.Database.Entities.Utils;
using Tusur.Practices.Persistence.UnitsOfWork;

namespace Tusur.Practices.Persistence.Services
{
    public class Service<TDomain, TPersistence> : IService<TDomain>
        where TDomain : DomainEntity
        where TPersistence : Entity, IMappable<TDomain, TPersistence>
    {
        private readonly IUnitOfWork _unitOfWork;

        public Service(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public RequestResult<IEnumerable<TDomain>> GetAll()
        {
            try
            {
                var repository = _unitOfWork.GetRepository<TPersistence>();
                var values = repository.GetAll();

                return new RequestResult<IEnumerable<TDomain>>
                {
                    Success = true,
                    Value = values.Select(IMappable<TDomain, TPersistence>.ToDomain)
                };
            } catch (Exception error)
            {
                return new RequestResult<IEnumerable<TDomain>>
                {
                    Success = false,
                    Error = error.Message
                };
            }
        }

        public RequestResult<IEnumerable<TDomain>> GetBy(Expression<Func<TDomain, bool>> predicate)
        {
            try
            {
                var repository = _unitOfWork.GetRepository<TPersistence>();
                var values = repository
                    .GetAll()
                    .Select(IMappable<TDomain, TPersistence>.ToDomain)
                    .AsQueryable().Where(predicate);

                return new RequestResult<IEnumerable<TDomain>>
                {
                    Success = true,
                    Value = values.ToList()
                };
            }
            catch (Exception error)
            {
                return new RequestResult<IEnumerable<TDomain>>
                {
                    Success = false,
                    Error = error.Message
                };
            }
        }

        public RequestResult<TDomain> Find(Guid id)
        {
            try
            {
                var repository = _unitOfWork.GetRepository<TPersistence>();
                var value = repository.Find(id);

                return new RequestResult<TDomain>
                {
                    Success = true,
                    Value = IMappable<TDomain, TPersistence>.ToDomain(value)
                };
            }
            catch (Exception error)
            {
                return new RequestResult<TDomain>
                {
                    Success = false,
                    Error = error.Message
                };
            }
        }

        public RequestResult<TDomain> Find(Expression<Func<TDomain, bool>> predicate)
        {
            try
            {
                var repository = _unitOfWork.GetRepository<TPersistence>();
                var value = repository
                    .GetAll()
                    .Select(IMappable<TDomain, TPersistence>.ToDomain)
                    .AsQueryable()
                    .FirstOrDefault(predicate);

                return new RequestResult<TDomain>
                {
                    Success = true,
                    Value = value
                };
            }
            catch (Exception error)
            {
                return new RequestResult<TDomain>
                {
                    Success = false,
                    Error = error.Message
                };
            }
        }

        public RequestResult<TDomain> Create(TDomain entity)
        {
            try
            {
                var repository = _unitOfWork.GetRepository<TPersistence>();

                var value = IMappable<TDomain, TPersistence>.FromDomain(entity);
                value = repository.Create(value);
                _unitOfWork.SaveChanges();

                entity.Id = value.Id;

                return new RequestResult<TDomain>
                {
                    Success = true,
                    Value = entity
                };
            }
            catch (Exception error)
            {
                return new RequestResult<TDomain>
                {
                    Success = false,
                    Error = error.Message
                };
            }
        }

        public RequestResult<TDomain> Update(TDomain entity)
        {
            try
            {
                var repository = _unitOfWork.GetRepository<TPersistence>();

                var value = IMappable<TDomain, TPersistence>.FromDomain(entity);
                value = repository.Update(value);
                _unitOfWork.SaveChanges();

                return new RequestResult<TDomain>
                {
                    Success = true,
                    Value = entity
                };
            }
            catch (Exception error)
            {
                return new RequestResult<TDomain>
                {
                    Success = false,
                    Error = error.Message
                };
            }
        }

        public RequestResult<TDomain> Remove(Guid id)
        {
            try
            {
                var repository = _unitOfWork.GetRepository<TPersistence>();

                var value = repository.Find(id);
                value = repository.Remove(value);
                _unitOfWork.SaveChanges();

                return new RequestResult<TDomain>
                {
                    Success = true,
                    Value = IMappable<TDomain, TPersistence>.ToDomain(value)
                };
            }
            catch (Exception error)
            {
                return new RequestResult<TDomain>
                {
                    Success = false,
                    Error = error.Message
                };
            }
        }
    }
}
