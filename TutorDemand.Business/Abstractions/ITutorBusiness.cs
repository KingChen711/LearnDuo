using System.Linq.Expressions;
using TutorDemand.Business.Base;
using TutorDemand.Data.Dtos.Tutor;
using TutorDemand.Data.Entities;

namespace TutorDemand.Business.Abstractions;

public interface ITutorBusiness
{
    Task<IBusinessResult> GetAllAsync();
    Task<IBusinessResult> FindOneAsync(Expression<Func<Tutor, bool>> condition);
    Task<IBusinessResult> CreateAsync(TutorDto entity);
    Task<IBusinessResult> UpdateAsync(TutorDto entity);
    Task<IBusinessResult> DeleteAsync(Guid tutorId);
}