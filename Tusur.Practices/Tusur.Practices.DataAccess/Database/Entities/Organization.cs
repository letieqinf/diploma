using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Tusur.Practices.Application.Domain.Entities;
using Tusur.Practices.Persistence.Database.Entities.Utils;

namespace Tusur.Practices.Persistence.Database.Entities
{
    [Index(nameof(Inn), nameof(Trrc), IsUnique = true)]
    public class Organization : Entity, IMappable<OrganizationEntity, Organization>
    {
        public long? Inn { get; set; }
        public long? Trrc { get; set; }

        public string OrganizationName { get; set; }
        public string OrganizationAddress { get; set; }

        public bool IsApproved { get; set; } = false;
    }
}
