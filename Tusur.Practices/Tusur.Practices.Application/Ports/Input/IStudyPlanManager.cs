using Tusur.Practices.Application.Domain.Entities;

namespace Tusur.Practices.Application.Ports.Input
{
    public interface IStudyPlanManager : IManager<StudyPlanEntity>, IManager<DegreeEntity>, IManager<StudyFormEntity>
    {
    }
}
