using System.Linq.Expressions;
using TutorDemand.Business.Base;
using TutorDemand.Data.Dtos.Tutor;
using TutorDemand.Data.Entities;

namespace TutorDemand.Business.Abstractions;

public interface ITutorBusiness 
{
    Task<IBusinessResult> GetAllAsync();
    Task<IBusinessResult> FindAsync(Expression<Func<Tutor, bool>> filter = null!,
        Func<IQueryable<Tutor>, IOrderedQueryable<Tutor>> orderBy = null!,
        string includeProperties = "");
    Task<IBusinessResult> FindOneAsync(Expression<Func<Tutor, bool>> expression);
    Task<IBusinessResult> CreateAsync(TutorAddDto entity);
    Task<IBusinessResult> UpdateAsync(TutorUpdateDto entity);
    Task<IBusinessResult> DeleteAsync(Guid tutorId);
}
