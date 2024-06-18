using Tusur.Practices.Application.Domain.Entities;

namespace Tusur.Practices.Application.Ports.Input
{
    public interface IStudyFieldManager : IManager<StudyFieldEntity>, IManager<SpecialtyEntity>
    {
    }
}
