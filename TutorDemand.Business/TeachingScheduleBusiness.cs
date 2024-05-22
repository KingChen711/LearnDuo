using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Mapster;
using Microsoft.EntityFrameworkCore;
using TutorDemand.Business.Abstractions;
using TutorDemand.Data.Dtos.TeachingSchedule;
using TutorDemand.Data.Entities;

namespace TutorDemand.Business
{
    public class TeachingScheduleBusiness : ITeachingScheduleBusiness
    {
        private readonly NET1704_221_5_TutorDemandContext _context;

        public TeachingScheduleBusiness(NET1704_221_5_TutorDemandContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TeachingSchedule>> GetAll() =>
            await _context.TeachingSchedules.ToListAsync();

        public async Task<IEnumerable<TeachingSchedule>> Find(
            Expression<Func<TeachingSchedule, bool>> expression
        ) => await _context.TeachingSchedules.Where(expression).ToListAsync();

        public async Task<TeachingSchedule?> FindOne(
            Expression<Func<TeachingSchedule, bool>> expression
        ) => await _context.TeachingSchedules.Where(expression).FirstOrDefaultAsync();

        public async Task Create(TeachingScheduleCreationDto dto)
        {
            var entity = dto.Adapt<TeachingSchedule>();

            _context.TeachingSchedules.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Update(TeachingScheduleUpdateDto dto)
        {
            var entity = dto.Adapt<TeachingSchedule>();

            _context.TeachingSchedules.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Guid teachingScheduleId)
        {
            var entity = await FindOne(t => t.TeachingScheduleId == teachingScheduleId);

            if (entity is null)
            {
                throw new Exception($"Not found Teaching Schedule with id: ${teachingScheduleId}");
            }

            _context.TeachingSchedules.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> Exist(Expression<Func<TeachingSchedule, bool>> expression) =>
            await _context.TeachingSchedules.AnyAsync(expression);
    }
}
