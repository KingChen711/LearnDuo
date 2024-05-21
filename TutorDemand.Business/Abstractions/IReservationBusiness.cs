using System.Linq.Expressions;
using TutorDemand.Data.Dtos.Reservation;
using TutorDemand.Data.Entities;

namespace TutorDemand.Business.Abstractions;

public interface IReservationBusiness
{
    Task<IEnumerable<Reservation>> GetAll();
    Task<IEnumerable<Reservation>> Find(Expression<Func<Reservation, bool>> expression);
    Task<Reservation?> FindOne(Expression<Func<Reservation, bool>> expression);
    Task Create(ReservationCreateDTO dto);
    Task Update(ReservationUpdateDTO dto);
    Task Delete(Guid reservationId);
    Task<bool> Exist(Expression<Func<Reservation, bool>> expression);
}