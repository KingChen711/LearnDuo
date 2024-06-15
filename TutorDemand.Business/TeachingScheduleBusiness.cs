using System.Linq.Expressions;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

        public async Task<IBusinessResult> GetAllAsync()
        {
            var teachingSchedules = await _unitOfWork
                .TeachingScheduleRepository.GetQueryable(false)
                .Include(tc => tc.Subject)
                .Include(tc => tc.Tutor)
                .Include(tc => tc.Slot)
                .ToListAsync();

            return new BusinessResult(
                Const.SUCCESS_READ_CODE,
                Const.SUCCESS_READ_MSG,
                teachingSchedules
            );
        }

        public async Task<IBusinessResult> GetDetail(Guid id)
        {
            var teachingSchedules = await _unitOfWork
                .TeachingScheduleRepository.GetQueryable(false)
                .Include(tc => tc.Subject)
                .Include(tc => tc.Tutor)
                .Include(tc => tc.Slot)
                .FirstOrDefaultAsync(tc => tc.TeachingScheduleId == id);

            return new BusinessResult(
                Const.SUCCESS_READ_CODE,
                Const.SUCCESS_READ_MSG,
                teachingSchedules
            );
        }

        public async Task<IBusinessResult> GetTeachingSchedulesAsync(QueryTeachingScheduleDto dto)
        {
            var pageSize = dto.PageSize;
            var pageNumber = dto.PageNumber;
            var searchTerm = dto.SearchTerm ?? "";

            var query = _unitOfWork
                .TeachingScheduleRepository.GetQueryable(false)
                .Include(ts => ts.Subject)
                .Include(ts => ts.Tutor)
                .Include(ts => ts.Slot)
                .Where(ts =>
                    ts.Subject.Name.ToLower().Contains(searchTerm.ToLower()) ||
                    ts.Tutor.Fullname.ToLower().Contains(searchTerm.ToLower()) ||
                    ts.Slot.SlotName.ToLower().Contains(searchTerm.ToLower()) ||
                    ts.MeetRoomCode.ToLower().Contains(searchTerm.ToLower()) ||
                    ts.RoomPassword.ToLower().Contains(searchTerm.ToLower()) ||
                    ts.LearnDays.ToLower().Contains(searchTerm.ToLower()));

            var teachingSchedulesWithMetaData = await PagedList<TeachingSchedule>.ToPagedList(
                query,
                pageNumber,
                pageSize
            );

            return new BusinessResult(
                Const.SUCCESS_READ_CODE,
                Const.SUCCESS_READ_MSG,
                teachingSchedulesWithMetaData
            );
        }

        public async Task<IBusinessResult> FindAsync(
            Expression<Func<TeachingSchedule, bool>> expression
        )
        {
            var teachingSchedules =
                await _unitOfWork.TeachingScheduleRepository.GetWithConditionAsync(expression);

            return new BusinessResult(
                Const.SUCCESS_READ_CODE,
                Const.SUCCESS_READ_MSG,
                teachingSchedules
            );
        }

        public async Task<IBusinessResult> FindOneAsync(
            Expression<Func<TeachingSchedule, bool>> expression
        )
        {
            var teachingSchedule =
                await _unitOfWork.TeachingScheduleRepository.GetOneWithConditionAsync(expression);

            if (teachingSchedule is null)
            {
                return new BusinessResult(
                    Const.FAIL_READ_CODE,
                    Const.FAIL_READ_MSG,
                    teachingSchedule
                );
            }

            return new BusinessResult(
                Const.SUCCESS_READ_CODE,
                Const.SUCCESS_READ_MSG,
                teachingSchedule
            );
        }

        public async Task<IBusinessResult> CreateAsync(TeachingScheduleMutationDto dto)
        {
            try
            {
                var entity = dto.Adapt<TeachingSchedule>();
                var result = await _unitOfWork.TeachingScheduleRepository.CreateAsync(entity) > 0;
                if (result)
                {
                    return new BusinessResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG);
                }
                else
                {
                    return new BusinessResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION_CODE, ex.Message);
            }
        }

        public async Task<IBusinessResult> UpdateAsync(Guid id, TeachingScheduleMutationDto dto)
        {
            var entity = await _unitOfWork.TeachingScheduleRepository.GetOneWithConditionAsync(t =>
                t.TeachingScheduleId == id
            );

            if (entity is null)
            {
                throw new Exception($"Not found Teaching Schedule with id: ${id}");
            }

            dto.Adapt(entity);

            await _unitOfWork.TeachingScheduleRepository.UpdateAsync(entity);

            return new BusinessResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG);
        }

        public async Task<IBusinessResult> DeleteAsync(Guid teachingScheduleId)
        {
            try
            {
                var entity = await _unitOfWork
                    .TeachingScheduleRepository.GetQueryable(true)
                    .Where(ts => ts.TeachingScheduleId == teachingScheduleId)
                    .Include(ts => ts.Reservations)
                    .FirstOrDefaultAsync();

                if (entity is null)
                {
                    return new BusinessResult(
                        Const.FAIL_DELETE_CODE,
                        $"Không tìm thấy lịch học với id: ${teachingScheduleId}"
                    );
                }

                if (entity.Reservations.Count > 0)
                {
                    return new BusinessResult(
                        Const.FAIL_DELETE_CODE,
                        $"Không thể xóa một lịch học đã có khách hàng đặt chỗ"
                    );
                }

                _unitOfWork.TeachingScheduleRepository.PrepareRemove(entity);
                await _unitOfWork.TeachingScheduleRepository.SaveAsync();

                return new BusinessResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG);
            }
            catch (Exception e)
            {
                return new BusinessResult(Const.FAIL_DELETE_CODE, Const.FAIL_READ_MSG);
            }
        }

        public async Task<IBusinessResult> GetWithConditionAsync(
            Expression<Func<TeachingSchedule, bool>> filter = null!,
            Func<IQueryable<TeachingSchedule>, IOrderedQueryable<TeachingSchedule>> orderBy = null!,
            string includeProperties = ""
        )
        {
            try
            {
                var teachingSchedules =
                    await _unitOfWork.TeachingScheduleRepository.GetWithConditionAsync(
                        filter,
                        orderBy,
                        includeProperties
                    );

                if (teachingSchedules.Any())
                {
                    return new BusinessResult(
                        Const.SUCCESS_READ_CODE,
                        Const.SUCCESS_READ_MSG,
                        teachingSchedules
                    );
                }
                else
                {
                    return new BusinessResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXCEPTION_CODE, ex.Message);
            }
        }
    }
}