using Tusur.Practices.Application.Domain.Entities;
using Tusur.Practices.Application.Ports.Input;
using Tusur.Practices.Application.Ports.Output;

namespace Tusur.Practices.Application.UseCases
{
    public class ApproveManager : IApproveManager
    {
        IService<ApprovedPracticeEntity> IManager<ApprovedPracticeEntity>.Service { get; set; }
        IService<ApprovedStudyPlanEntity> IManager<ApprovedStudyPlanEntity>.Service { get; set; }

        public ApproveManager(IService<ApprovedPracticeEntity> approvedPracticeService, IService<ApprovedStudyPlanEntity> approvedStudyPlanService)
        {
            ((IManager<ApprovedPracticeEntity>)this).Service = approvedPracticeService;
            ((IManager<ApprovedStudyPlanEntity>)this).Service = approvedStudyPlanService;
        }
    }
}
