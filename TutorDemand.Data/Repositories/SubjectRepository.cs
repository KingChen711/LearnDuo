using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TutorDemand.Data.Base;
using TutorDemand.Data.Entities;

namespace TutorDemand.Data.Repositories
{
    public class SubjectRepository : GenericRepository<Subject>
    {
        public SubjectRepository() { }

        public SubjectRepository(NET1704_221_5_TutorDemandContext context) => _context = context;

        public IEnumerable<Subject> GetWithCondition(
            Expression<Func<Subject, bool>> filter = null!,
            Func<IQueryable<Subject>, IOrderedQueryable<Subject>> orderBy = null!,
            string includeProperties = "")
        {
            IQueryable<Subject> query = GetQueryable();

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

        public async Task<IEnumerable<Subject>> GetWithConditionAsync(
            Expression<Func<Subject, bool>> filter = null!,
            Func<IQueryable<Subject>, IOrderedQueryable<Subject>> orderBy = null!,
            string includeProperties = "")
        {
            IQueryable<Subject> query = GetQueryable();

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
}