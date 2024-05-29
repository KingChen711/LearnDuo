using System.Linq;
using System.Linq.Expressions;
using TutorDemand.Business.Base;
using TutorDemand.Data.Dtos.Subject;
using TutorDemand.Data.Entities;

namespace TutorDemand.Business.Abstractions;

public interface ISubjectBusiness
{

    //  Summary:
    //      Synchronous methods 
    public IBusinessResult Create(SubjectDto subject);
    public IBusinessResult Delete(Guid subjectId);
    public IBusinessResult Delete(int id);
    public IBusinessResult Update(SubjectDto subject);
    public IBusinessResult GetById(Guid subjectId);
    public IBusinessResult GetById(int id);
    public IBusinessResult GetAll();
    public IBusinessResult GetWithCondition(
        Expression<Func<Subject, bool>> filter = null!,
        Func<IQueryable<Subject>, IOrderedQueryable<Subject>> orderBy = null!,
        string includeProperties = "");

    //  Summary:
    //      Asynchronous methods 
    public Task<IBusinessResult> CreateAsync(SubjectDto subject);
    public Task<IBusinessResult> DeleteAsync(Guid subjectId);
    public Task<IBusinessResult> UpdateAsync(SubjectDto subject);
    public Task<IBusinessResult> GetByIdAsync(Guid subjectId);  
    public Task<IBusinessResult> GetAllAsync();
    public Task<IBusinessResult> GetWithConditionAysnc(
        Expression<Func<Subject, bool>> filter = null!,
        Func<IQueryable<Subject>, IOrderedQueryable<Subject>> orderBy = null!,
        string includeProperties = "");

    public int SaveChanges();
    public Task<int> SaveChangeAsync();
}