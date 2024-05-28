using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TutorDemand.Data.Base;
using TutorDemand.Data.Entities;

namespace TutorDemand.Data.Repositories
{
    public class SubjectRepository : GenericRepository<Subject>
    {
        public IEnumerable<Subject> GetWithCondition(
            Expression<Func<Subject, bool>> filter = null!,
            Func<IQueryable<Subject>, IOrderedQueryable<Subject>> orderBy = null!,
            string includeProperties = "")
        {
            IQueryable<Subject> query = _dbSet;

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
            IQueryable<Subject> query = _dbSet;

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
