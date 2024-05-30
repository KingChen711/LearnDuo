using System.Linq.Expressions;
using TutorDemand.Business.Base;
using TutorDemand.Data.Dtos.TeachingSchedule;
using TutorDemand.Data.Entities;

namespace TutorDemand.Business.Abstractions;

public interface ITeachingScheduleBusiness
{
    Task<IBusinessResult> GetAll();
    Task<IBusinessResult> GetTeachingSchedules(QueryTeachingScheduleDto dto);
    Task<IBusinessResult> Find(Expression<Func<TeachingSchedule, bool>> expression);
    Task<IBusinessResult> FindOne(Expression<Func<TeachingSchedule, bool>> expression);
    Task<IBusinessResult> Create(TeachingScheduleMutationDto entity);
    Task<IBusinessResult> Update(Guid id, TeachingScheduleMutationDto entity);
    Task<IBusinessResult> Delete(Guid teachingScheduleId);
}
