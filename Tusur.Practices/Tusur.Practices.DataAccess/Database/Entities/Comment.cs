using System.ComponentModel.DataAnnotations.Schema;
using Tusur.Practices.Application.Domain.Entities;
using Tusur.Practices.Persistence.Database.Entities.Utils;

namespace Tusur.Practices.Persistence.Database.Entities
{
    public class Comment : Entity, IMappable<CommentEntity, Comment>
    {
        [ForeignKey("User")] public Guid SenderId { get; set; }
        public User User { get; set; }

        public Guid ApplicationId { get; set; }
        public Application Application { get; set; }

        public string Content { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
