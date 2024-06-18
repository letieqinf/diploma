using Microsoft.EntityFrameworkCore;
using Tusur.Practices.Application.Domain.Entities;
using Tusur.Practices.Persistence.Database.Entities.Utils;

namespace Tusur.Practices.Persistence.Database.Entities
{
    [Index(nameof(IsDefault), IsUnique = true)]
    public class ContractContent : Entity, IMappable<ContractContentEntity, ContractContent>
    {
        public string UniResp { get; set; }
        public string OrgResp { get; set; }
        public string UniCan { get; set; }
        public string OrgCan { get; set; }

        public bool? IsDefault { get; set; } = null;
    }
}
