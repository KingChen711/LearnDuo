using TutorDemand.Business.Abstractions;
using TutorDemand.Business.Base;
using TutorDemand.Common;
using TutorDemand.Data;

namespace TutorDemand.Business;

public class SlotBusiness : ISlotBusiness
{
    private readonly UnitOfWork _unitOfWork;

    public SlotBusiness()
    {
        _unitOfWork ??= new UnitOfWork();
    }

    public async Task<IBusinessResult> GetAllAsync()
    {
        var teachingSchedules = await _unitOfWork.SlotRepository.GetAllAsync();

        return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, teachingSchedules);
    }
}