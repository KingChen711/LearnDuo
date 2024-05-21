using System.Linq.Expressions;
using TutorDemand.Data.Dtos.Tutor;
using TutorDemand.Data.Entities;

namespace TutorDemand.Business.Abstractions;

public interface ITutorBusiness 
{
    Task<IEnumerable<Tutor>> GetAll();
    Task<IEnumerable<Tutor>> Find(Expression<Func<Tutor, bool>> expression);
    Task<Tutor?> FindOne(Expression<Func<Tutor, bool>> expression);
    Task Create(TutorAddDto entity);
    Task Update(TutorUpdateDto entity);
    Task Delete(Guid tutorId);
}
