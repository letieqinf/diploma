using Tusur.Practices.Application.Domain.Entities;

namespace Tusur.Practices.Persistence.Database.Entities.Utils
{
    public interface IMappable<TDomain, TPersistence>
        where TDomain : IDomainEntity
        where TPersistence : IEntity
    {
        public static Type DomainType { get; } = typeof(TDomain);
        public static Type PersistenceType { get; } = typeof(TPersistence);

        public static TDomain ToDomain(TPersistence entity)
        {
            var props = entity.GetType().GetProperties();
            var instance = Activator.CreateInstance(typeof(TDomain))
                ?? throw new NullReferenceException();

            foreach (var property in props)
            {
                if (((TDomain)instance).GetType().GetProperty(property.Name) == null)
                    continue;

                var value = property.GetValue(entity, null);
                ((TDomain)instance)
                    .GetType()
                    .GetProperty(property.Name)!
                    .SetValue((TDomain)instance, value, null);
            }

            return (TDomain)instance;
        }

        public static TPersistence FromDomain(TDomain entity)
        {
            var props = entity.GetType().GetProperties();
            var instance = Activator.CreateInstance(typeof(TPersistence))
                ?? throw new NullReferenceException();

            foreach (var property in props)
            {
                if (((TPersistence)instance).GetType().GetProperty(property.Name) == null)
                    continue;

                var value = property.GetValue(entity, null);
                ((TPersistence)instance)
                    .GetType()
                    .GetProperty(property.Name)!
                    .SetValue((TPersistence)instance, value, null);
            }

            return (TPersistence)instance;
        }
    }
}
