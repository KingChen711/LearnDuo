using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Mapster;
using Microsoft.EntityFrameworkCore;
using TutorDemand.Business.Abstractions;
using TutorDemand.Business.Base;
using TutorDemand.Common;
using TutorDemand.Data;
using TutorDemand.Data.DAO;
using TutorDemand.Data.Dtos.TeachingSchedule;
using TutorDemand.Data.Entities;

namespace TutorDemand.Business
{
    public class TeachingScheduleBusiness : ITeachingScheduleBusiness
    {
        //private readonly TeachingScheduleDAO _unitOfWork.TeachingScheduleRepository;
        public UnitOfWork _unitOfWork { get; set; }
        public TeachingScheduleBusiness()
        {
            //_unitOfWork.TeachingScheduleRepository = new TeachingScheduleDAO();
            _unitOfWork = new UnitOfWork();
        }

        //public async Task<IEnumerable<TeachingSchedule>> GetAll() =>
        //    await _unitOfWork.TeachingScheduleRepository.GetAllAsync();

        //public async Task<IEnumerable<TeachingSchedule>> Find(
        //    Expression<Func<TeachingSchedule, bool>> expression
        //) => await _unitOfWork.TeachingScheduleRepository.GetWithConditionAsync(expression);

        //public async Task<TeachingSchedule?> FindOne(
        //    Expression<Func<TeachingSchedule, bool>> expression
        //) => await _unitOfWork.TeachingScheduleRepository.FindOneAsync(expression);


        public async Task Create(TeachingScheduleCreationDto dto)
        {
            var entity = dto.Adapt<TeachingSchedule>();

            await _unitOfWork.TeachingScheduleRepository.CreateAsync(entity);
        }

        //public async Task Update(TeachingScheduleUpdateDto dto)
        //{
        //    var entity = dto.Adapt<TeachingSchedule>();

        //    await _unitOfWork.TeachingScheduleRepository.UpdateAsync(entity);
        //}

        //public async Task Delete(Guid teachingScheduleId)
        //{
        //    var entity = await FindOne(t => t.TeachingScheduleId == teachingScheduleId);

        //    if (entity is null)
        //    {
        //        throw new Exception($"Not found Teaching Schedule with id: ${teachingScheduleId}");
        //    }

        //    await _unitOfWork.TeachingScheduleRepository.RemoveAsync(entity);
        //}

        //public async Task<bool> Exist(Expression<Func<TeachingSchedule, bool>> expression) =>
        //    await _unitOfWork.TeachingScheduleRepository.ExistAsync(expression);

        public async Task<IBusinessResult> GetWithConditionAysnc(
        Expression<Func<TeachingSchedule, bool>> filter = null!,
        Func<IQueryable<TeachingSchedule>, IOrderedQueryable<TeachingSchedule>> orderBy = null!,
        string includeProperties = "")
        {
            try
            {
                var teachingSchedules = await _unitOfWork.TeachingScheduleRepository.GetWithConditionAsync(filter, orderBy, includeProperties);

                if (teachingSchedules.Any())
                {
                    return new BusinessResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, teachingSchedules);
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
