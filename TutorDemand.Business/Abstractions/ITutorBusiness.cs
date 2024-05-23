using System.Linq.Expressions;
using TutorDemand.Business.Base;
using TutorDemand.Data.Dtos.Tutor;
using TutorDemand.Data.Entities;

namespace TutorDemand.Business.Abstractions;

public interface ITutorBusiness 
{
    Task<IBusinessResult> GetAll();
    Task<IBusinessResult> Find(Expression<Func<Tutor, bool>> filter = null!,
        Func<IQueryable<Tutor>, IOrderedQueryable<Tutor>> orderBy = null!,
        string includeProperties = "");
    Task<IBusinessResult> FindOne(Expression<Func<Tutor, bool>> expression);
    Task<IBusinessResult> Create(TutorAddDto entity);
    Task<IBusinessResult> Update(TutorUpdateDto entity);
    Task<IBusinessResult> Delete(Guid tutorId);
}
