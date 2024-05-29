using System.Linq.Expressions;
using TutorDemand.Data.Dtos.TeachingSchedule;
using TutorDemand.Data.Entities;

namespace TutorDemand.Business.Abstractions;

public interface ITeachingScheduleBusiness
{
    Task<IEnumerable<TeachingSchedule>> GetAll();
    Task<IEnumerable<TeachingSchedule>> Find(Expression<Func<TeachingSchedule, bool>> expression);
    Task<TeachingSchedule?> FindOne(Expression<Func<TeachingSchedule, bool>> expression);
    Task Create(TeachingScheduleCreationDto entity);
    Task Update(TeachingScheduleUpdateDto entity);
    Task Delete(Guid teachingScheduleId);
    Task<bool> Exist(Expression<Func<TeachingSchedule, bool>> expression);
}