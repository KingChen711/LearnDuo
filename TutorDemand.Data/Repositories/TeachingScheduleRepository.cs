using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TutorDemand.Data.Base;
using TutorDemand.Data.Entities;

namespace TutorDemand.Data.Repositories
{
    public class TeachingScheduleRepository : GenericRepository<TeachingSchedule>
    {
        public TeachingScheduleRepository(NET1704_221_5_TutorDemandContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TeachingSchedule>> GetWithConditionAsync(
            Expression<Func<TeachingSchedule, bool>> filter = null!,
            Func<IQueryable<TeachingSchedule>, IOrderedQueryable<TeachingSchedule>> orderBy = null!,
            string includeProperties = "")
        {
            IQueryable<TeachingSchedule> query = GetQueryable();

            if (filter != null)
                query = query.Where(filter);

            foreach (var includeProperty in includeProperties.Split(
                         new char[] { ',' },
                         StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
                return await orderBy(query).ToListAsync();
            else
                return await query.ToListAsync();
        }
    }
}