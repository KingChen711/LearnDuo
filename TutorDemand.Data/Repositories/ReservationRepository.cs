using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TutorDemand.Data.Base;
using TutorDemand.Data.Entities;

namespace TutorDemand.Data.Repositories;

public class ReservationRepository : GenericRepository<Reservation>
{
    public ReservationRepository() { }
    
            public ReservationRepository(NET1704_221_5_TutorDemandContext context) => _context = context;
    
            public IEnumerable<Reservation> GetWithCondition(
                Expression<Func<Reservation, bool>> filter = null!,
                Func<IQueryable<Reservation>, IOrderedQueryable<Reservation>> orderBy = null!,
                string includeProperties = "")
            {
                IQueryable<Reservation> query = GetQueryable();
    
                if (filter != null)
                    query = query.Where(filter);
    
                foreach (
                    var includeProperty in includeProperties.Split(
                        new char[] { ',' },
                        StringSplitOptions.RemoveEmptyEntries
                    )
                )
                {
                    query = query.Include(includeProperty);
                }
    
                if (orderBy != null)
                    return orderBy(query).ToList();
                else
                    return query.ToList();
            }
    
            public async Task<IEnumerable<Reservation>> GetWithConditionAsync(
                Expression<Func<Reservation, bool>> filter = null!,
                Func<IQueryable<Reservation>, IOrderedQueryable<Reservation>> orderBy = null!,
                string includeProperties = "")
            {
                IQueryable<Reservation> query = GetQueryable();
    
                if (filter != null)
                    query = query.Where(filter);
    
                if (includeProperties != null)
                {
                    foreach (var includeProperty in includeProperties.Split(
                        new char[] { ',' },
                        StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(includeProperty);
                    }
                }
    
                if (orderBy != null)
                    return await orderBy(query).ToListAsync();
                else
                    return await query.ToListAsync();
            }
}