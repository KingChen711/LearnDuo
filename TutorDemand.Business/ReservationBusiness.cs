using System.Linq.Expressions;
using Mapster;
using Microsoft.EntityFrameworkCore;
using TutorDemand.Business.Abstractions;
using TutorDemand.Data.Dtos.Reservation;
using TutorDemand.Data.Entities;

namespace TutorDemand.Business;

public class ReservationBusiness : IReservationBusiness
{
    private readonly TutorDemandContext _context;

    public ReservationBusiness(TutorDemandContext context)
    {
        _context = context;
    }


    public async Task<IEnumerable<Reservation>> GetAll()
    {
        return await _context.Reservations.ToListAsync();
    }

    public async Task<IEnumerable<Reservation>> Find(Expression<Func<Reservation, bool>> expression)
        => await _context.Reservations.Where(expression).ToListAsync();

    public async Task<Reservation?> FindOne(Expression<Func<Reservation, bool>> expression)
        => await _context.Reservations.Where(expression).FirstOrDefaultAsync();

    public async Task Create(ReservationCreateDto dto)
    {
        var entity = dto.Adapt<Reservation>();

        _context.Reservations.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Update(ReservationUpdateDto dto)
    {
        var entity = dto.Adapt<Reservation>();
        _context.Reservations.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(Guid reservationId)
    {
        var entity = await FindOne(r => r.ReservationId.Equals(reservationId));
        if (entity is null)
        {
            throw new Exception($"Not found Reservation with id ; {reservationId}");
        }

        _context.Reservations.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> Exist(Expression<Func<Reservation, bool>> expression)
        => await _context.Reservations.AnyAsync(expression);
}