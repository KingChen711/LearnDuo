using TutorDemand.Business.Base;

namespace TutorDemand.Business.Abstractions;

public interface ISlotBusiness
{
    Task<IBusinessResult> GetAllAsync();
    IBusinessResult GetAll();
}