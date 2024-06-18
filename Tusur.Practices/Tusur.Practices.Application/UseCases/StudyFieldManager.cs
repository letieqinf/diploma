using Tusur.Practices.Application.Domain.Entities;
using Tusur.Practices.Application.Ports.Input;
using Tusur.Practices.Application.Ports.Output;

namespace Tusur.Practices.Application.UseCases
{
    public class StudyFieldManager : IStudyFieldManager
    {

        IService<StudyFieldEntity> IManager<StudyFieldEntity>.Service { get; set; }
        IService<SpecialtyEntity> IManager<SpecialtyEntity>.Service { get; set; }

        public StudyFieldManager(
            IService<StudyFieldEntity> studyFieldService,
            IService<SpecialtyEntity> specialtyService)
        {
            ((IManager<StudyFieldEntity>)this).Service = studyFieldService;
            ((IManager<SpecialtyEntity>)this).Service = specialtyService;
        }
    }
}
