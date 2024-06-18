using System.ComponentModel.DataAnnotations;
using Tusur.Practices.Application.Domain.Entities;
using Tusur.Practices.Persistence.Database.Entities.Utils;

namespace Tusur.Practices.Persistence.Database.Entities
{
    public class Notification : Entity, IMappable<NotificationEntity, Notification>
    {
        public Guid UserId { get; set; }
        public User User { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsChecked { get; set; }
        public DateTime SentAt { get; set; }
    }
}
