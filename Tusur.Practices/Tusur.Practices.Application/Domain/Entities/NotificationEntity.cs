using System.ComponentModel.DataAnnotations;

namespace Tusur.Practices.Application.Domain.Entities
{
    public class NotificationEntity : DomainEntity
    {
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsChecked { get; set; }
        public DateTime SentAt { get; set; }
    }
}
