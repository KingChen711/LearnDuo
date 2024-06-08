using System.Linq.Expressions;
using TutorDemand.Business.Base;
using TutorDemand.Data.Dtos.TeachingSchedule;
using TutorDemand.Data.Entities;

namespace TutorDemand.Business.Abstractions;

public interface ITeachingScheduleBusiness
{
    Task<IBusinessResult> GetAllAsync();
    Task<IBusinessResult> GetTeachingSchedulesAsync(QueryTeachingScheduleDto dto);
    Task<IBusinessResult> FindAsync(Expression<Func<TeachingSchedule, bool>> expression);
    Task<IBusinessResult> FindOneAsync(Expression<Func<TeachingSchedule, bool>> expression);
    Task<IBusinessResult> CreateAsync(TeachingScheduleMutationDto entity);
    Task<IBusinessResult> UpdateAsync(Guid id, TeachingScheduleMutationDto entity);
    Task<IBusinessResult> DeleteAsync(Guid teachingScheduleId);

    Task<IBusinessResult> GetWithConditionAsync(
       Expression<Func<TeachingSchedule, bool>> filter = null!,
       Func<IQueryable<TeachingSchedule>, IOrderedQueryable<TeachingSchedule>> orderBy = null!,
       string includeProperties = "");
}
