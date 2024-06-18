using System.ComponentModel.DataAnnotations.Schema;
using Tusur.Practices.Application.Domain.Entities;
using Tusur.Practices.Persistence.Database.Entities.Utils;

namespace Tusur.Practices.Persistence.Database.Entities
{
    public class Contract : Entity, IMappable<ContractEntity, Contract>
    {
        public Guid OrganizationId { get; set; }
        public Organization Organization { get; set; }

        public Guid ContractContentId { get; set; }
        public ContractContent ContractContent { get; set; }

        public bool IsLongTerm { get; set; } = false;
    }
}
