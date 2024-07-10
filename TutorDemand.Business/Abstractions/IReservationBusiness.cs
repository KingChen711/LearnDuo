using System.Linq.Expressions;
using TutorDemand.Business.Base;
using TutorDemand.Data.Dtos.Reservation;
using TutorDemand.Data.Entities;

namespace TutorDemand.Business.Abstractions;

public interface IReservationBusiness
{
    IBusinessResult GetWithCondition(
        Expression<Func<Reservation, bool>> filter = null!,
        Func<IQueryable<Reservation>, IOrderedQueryable<Reservation>> orderBy = null!,
        string includeProperties = "");

    Task<IBusinessResult> GetWithConditionAysnc(
        Expression<Func<Reservation, bool>> filter = null!,
        Func<IQueryable<Reservation>, IOrderedQueryable<Reservation>> orderBy = null!,
        string includeProperties = "");
    IBusinessResult Create(Reservation reservation);
    public IBusinessResult Delete(Guid reservationId);
    IBusinessResult Delete(int id);
    IBusinessResult Update(Reservation reservation);
    public IBusinessResult GetById(Guid reservationId);
    public IBusinessResult GetById(int id);
    public IBusinessResult GetAll();
    // public IBusinessResult GetWithCondition(
    //     Expression<Func<Reservation, bool>> filter = null!,
    //     Func<IQueryable<Reservation>, IOrderedQueryable<Reservation>> orderBy = null!,
    //     string includeProperties = "");

    Task<IBusinessResult> FindOneAsync(Expression<Func<Reservation, bool>> condition);

    public Task<IBusinessResult> CreateAsync(Reservation reservation);
    public Task<IBusinessResult> DeleteAsync(Guid reservationId);
    public Task<IBusinessResult> UpdateAsync(Reservation reservation);
    public Task<IBusinessResult> GetByIdAsync(Guid reservationId);
    public Task<IBusinessResult> GetAllAsync();
    // public Task<IBusinessResult> GetWithConditionAysnc(
    //     Expression<Func<Reservation, bool>> filter = null!,
    //     Func<IQueryable<Reservation>, IOrderedQueryable<Reservation>> orderBy = null!,
    //     string includeProperties = "");
    
    public int SaveChanges();
    public Task<int> SaveChangeAsync();
}