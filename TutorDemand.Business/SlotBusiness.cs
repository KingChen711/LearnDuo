using AutoMapper;
using TutorDemand.Business.Abstractions;
using TutorDemand.Business.Base;
using TutorDemand.Common;
using TutorDemand.Data;
using TutorDemand.Data.Dtos.Slot;
using TutorDemand.Data.Entities;
using TutorDemand.Data.Mappings;

namespace TutorDemand.Business;

public class SlotBusiness : ISlotBusiness
{
    private readonly UnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public SlotBusiness()
    {
        _unitOfWork ??= new UnitOfWork();
        _mapper = new MapperConfiguration(mc =>
        {
            mc.AddProfile<ProfilesMapper>();
        }).CreateMapper();
    }

    public async Task<IBusinessResult> GetAllAsync()
    {
        var teachingSchedules = await _unitOfWork.SlotRepository.GetAllAsync();

        return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, teachingSchedules);
    }

    public IBusinessResult GetAll()
    {
        var slots = _unitOfWork.SlotRepository.GetAll();

        return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, _mapper.Map<List<SlotDto>>(slots));
    }
}