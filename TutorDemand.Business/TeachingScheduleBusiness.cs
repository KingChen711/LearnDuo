using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Mapster;
using Microsoft.EntityFrameworkCore;
using TutorDemand.Business.Abstractions;
using TutorDemand.Data;
using TutorDemand.Data.DAO;
using TutorDemand.Data.Dtos.TeachingSchedule;
using TutorDemand.Data.Entities;
using TutorDemand.Data.Repositories;

namespace TutorDemand.Business
{
    public class TeachingScheduleBusiness : ITeachingScheduleBusiness
    {
        private readonly UnitOfWork _unitOfWork;

        public TeachingScheduleBusiness()
        {
            _unitOfWork ??= new UnitOfWork();
        }

        public async Task<IEnumerable<TeachingSchedule>> GetAll() =>
            await _unitOfWork.TeachingScheduleRepository.GetAllAsync();

        public async Task<IEnumerable<TeachingSchedule>> Find(
            Expression<Func<TeachingSchedule, bool>> expression
        ) => await _unitOfWork.TeachingScheduleRepository.GetWithConditionAsync(expression);

        public async Task<TeachingSchedule?> FindOne(
            Expression<Func<TeachingSchedule, bool>> expression
        ) => await _unitOfWork.TeachingScheduleRepository.GetOneWithConditionAsync(expression);

        public async Task Create(TeachingScheduleCreationDto dto)
        {
            var entity = dto.Adapt<TeachingSchedule>();

            await _unitOfWork.TeachingScheduleRepository.CreateAsync(entity);
        }

        public async Task Update(TeachingScheduleUpdateDto dto)
        {
            var entity = dto.Adapt<TeachingSchedule>();

            await _unitOfWork.TeachingScheduleRepository.UpdateAsync(entity);
        }

        public async Task Delete(Guid teachingScheduleId)
        {
            var entity = await FindOne(t => t.TeachingScheduleId == teachingScheduleId);

            if (entity is null)
            {
                throw new Exception($"Not found Teaching Schedule with id: ${teachingScheduleId}");
            }

            await _unitOfWork.TeachingScheduleRepository.RemoveAsync(entity);
        }

        public async Task<bool> Exist(Expression<Func<TeachingSchedule, bool>> expression) =>
            await _unitOfWork.TeachingScheduleRepository.ExistAsync(expression);
    }
}