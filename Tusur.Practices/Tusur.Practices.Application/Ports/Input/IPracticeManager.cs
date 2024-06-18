using Tusur.Practices.Application.Domain.Entities;
using Tusur.Practices.Application.Domain.Models.Result;

namespace Tusur.Practices.Application.Ports.Input
{
    public interface IPracticeManager : IManager<PracticeKindEntity>, IManager<PracticeTypeEntity>, IManager<PracticeEntity>, IManager<PracticeDateEntity>
    {

    }
}
