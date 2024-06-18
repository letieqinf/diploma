namespace Tusur.Practices.Application.Domain.Entities
{
    public class CommentEntity : DomainEntity
    {
        public Guid SenderId { get; set; }
        public Guid ApplicationId { get; set; }
        public string Content { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
