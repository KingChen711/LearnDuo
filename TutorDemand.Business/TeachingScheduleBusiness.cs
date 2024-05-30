using System.Linq.Expressions;
using Mapster;
using Microsoft.EntityFrameworkCore;
using TutorDemand.Business.Abstractions;
using TutorDemand.Business.Base;
using TutorDemand.Common;
using TutorDemand.Data;
using TutorDemand.Data.Dtos;
using TutorDemand.Data.Dtos.TeachingSchedule;
using TutorDemand.Data.Entities;

namespace TutorDemand.Business
{
    public class TeachingScheduleBusiness : ITeachingScheduleBusiness
    {
        private readonly UnitOfWork _unitOfWork;

        public TeachingScheduleBusiness()
        {
            _unitOfWork ??= new UnitOfWork();
        }

        public async Task<IBusinessResult> GetAll()
        {
            var teachingSchedules = await _unitOfWork.TeachingScheduleRepository.GetAllAsync();

            return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, teachingSchedules);
        }

        public async Task<IBusinessResult> GetTeachingSchedules(QueryTeachingScheduleDto dto)
        {
            var pageSize = dto.PageSize;
            var pageNumber = dto.PageNumber;

            var query = _unitOfWork.TeachingScheduleRepository
                .GetQueryable(false)
                .Include(ts => ts.Subject)
                .Include(ts => ts.Tutor)
                .Include(ts => ts.Slot);

            var teachingSchedulesWithMetaData =
                await PagedList<TeachingSchedule>.ToPagedList(query, pageNumber, pageSize);

            return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, teachingSchedulesWithMetaData);
        }

        public async Task<IBusinessResult> Find(
            Expression<Func<TeachingSchedule, bool>> expression
        )
        {
            var teachingSchedules = await _unitOfWork.TeachingScheduleRepository.GetWithConditionAsync(expression);

            return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, teachingSchedules);
        }

        public async Task<IBusinessResult> FindOne(
            Expression<Func<TeachingSchedule, bool>> expression
        )
        {
            var teachingSchedule = await _unitOfWork.TeachingScheduleRepository.GetOneWithConditionAsync(expression);

            if (teachingSchedule is null)
            {
                return new BusinessResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG, teachingSchedule);
            }

            return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, teachingSchedule);
        }


        public async Task<IBusinessResult> Create(TeachingScheduleMutationDto dto)
        {
            var entity = dto.Adapt<TeachingSchedule>();

            await _unitOfWork.TeachingScheduleRepository.CreateAsync(entity);

            return new BusinessResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG);
        }

        public async Task<IBusinessResult> Update(Guid id, TeachingScheduleMutationDto dto)
        {
            var entity =
                await _unitOfWork.TeachingScheduleRepository.GetOneWithConditionAsync(t =>
                    t.TeachingScheduleId == id);

            if (entity is null)
            {
                throw new Exception($"Not found Teaching Schedule with id: ${id}");
            }

            dto.Adapt(entity);

            await _unitOfWork.SaveChangesAsync();

            return new BusinessResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG);
        }

        public async Task<IBusinessResult> Delete(Guid teachingScheduleId)
        {
            try
            {
                var entity =
                    await _unitOfWork.TeachingScheduleRepository
                        .GetQueryable(true)
                        .Where(ts => ts.TeachingScheduleId == teachingScheduleId)
                        .Include(ts => ts.Reservations).FirstOrDefaultAsync();

                if (entity is null)
                {
                    return new BusinessResult(Const.FAIL_DELETE_CODE,
                        $"Không tìm thấy lịch học với id: ${teachingScheduleId}");
                }

                if (entity.Reservations.Count > 0)
                {
                    return new BusinessResult(Const.FAIL_DELETE_CODE,
                        $"Không thể xóa một lịch học đã có khách hàng đặt chỗ");
                }

                await _unitOfWork.TeachingScheduleRepository.RemoveAsync(entity);

                return new BusinessResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG);
            }
            catch (Exception e)
            {
                return new BusinessResult(Const.FAIL_DELETE_CODE, Const.FAIL_READ_MSG);
            }
        }
    }
}