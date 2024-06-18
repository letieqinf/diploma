namespace Tusur.Practices.Persistence.Database.Entities.Utils
{
    public interface IEntity : IDisposable
    {
        Guid Id { get; set; }
    }
}
