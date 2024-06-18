using Tusur.Practices.Application.Domain.Entities;
using Tusur.Practices.Application.Domain.Models.Result;
using Tusur.Practices.Application.Ports.Input;
using Tusur.Practices.Application.Ports.Output;

namespace Tusur.Practices.Application.UseCases
{
    public class PracticeManager : IPracticeManager
    {
        IService<PracticeEntity> IManager<PracticeEntity>.Service { get; set; }
        IService<PracticeDateEntity> IManager<PracticeDateEntity>.Service { get; set; }
        IService<PracticeKindEntity> IManager<PracticeKindEntity>.Service { get; set; }
        IService<PracticeTypeEntity> IManager<PracticeTypeEntity>.Service { get; set; }

        public PracticeManager(
            IService<PracticeEntity> practiceService,
            IService<PracticeDateEntity> practiceDateService,
            IService<PracticeKindEntity> practiceKindService,
            IService<PracticeTypeEntity> practiceTypeService)
        {
            ((IManager<PracticeEntity>)this).Service = practiceService;
            ((IManager<PracticeDateEntity>)this).Service = practiceDateService;
            ((IManager<PracticeKindEntity>)this).Service = practiceKindService;
            ((IManager<PracticeTypeEntity>)this).Service = practiceTypeService;
        }
    }
}
