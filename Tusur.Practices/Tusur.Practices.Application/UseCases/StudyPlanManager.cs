using Tusur.Practices.Application.Domain.Entities;
using Tusur.Practices.Application.Ports.Input;
using Tusur.Practices.Application.Ports.Output;

namespace Tusur.Practices.Application.UseCases
{
    public class StudyPlanManager : IStudyPlanManager
    {
        IService<StudyPlanEntity> IManager<StudyPlanEntity>.Service { get; set; }
        IService<DegreeEntity> IManager<DegreeEntity>.Service { get; set; }
        IService<StudyFormEntity> IManager<StudyFormEntity>.Service { get; set; }

        public StudyPlanManager(
            IService<StudyPlanEntity> studyPlanService,
            IService<DegreeEntity> degreeService,
            IService<StudyFormEntity> studyFormService)
        {
            ((IManager<StudyPlanEntity>)this).Service = studyPlanService;
            ((IManager<DegreeEntity>)this).Service = degreeService;
            ((IManager<StudyFormEntity>)this).Service = studyFormService;
        }
    }
}
