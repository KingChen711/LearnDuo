using System.Linq.Expressions;
using TutorDemand.Business.Base;
using TutorDemand.Data.Dtos.Reservation;
using TutorDemand.Data.Entities;

namespace TutorDemand.Business.Abstractions;

public interface IReservationBusiness
{
    public IBusinessResult Create(Reservation reservation);
    public IBusinessResult Delete(Guid reservationId);
    public IBusinessResult Delete(int id);
    public IBusinessResult Update(Reservation reservation);
    public IBusinessResult GetById(Guid reservationId);
    public IBusinessResult GetById(int id);
    public IBusinessResult GetAll();
    // public IBusinessResult GetWithCondition(
    //     Expression<Func<Reservation, bool>> filter = null!,
    //     Func<IQueryable<Reservation>, IOrderedQueryable<Reservation>> orderBy = null!,
    //     string includeProperties = "");

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