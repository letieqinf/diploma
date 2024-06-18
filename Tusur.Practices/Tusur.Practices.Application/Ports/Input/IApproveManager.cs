using Tusur.Practices.Application.Domain.Entities;

namespace Tusur.Practices.Application.Ports.Input
{
    public interface IApproveManager : IManager<ApprovedPracticeEntity>, IManager<ApprovedStudyPlanEntity>
    {

    }
}
