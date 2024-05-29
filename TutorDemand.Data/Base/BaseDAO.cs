using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TutorDemand.Data.Entities;

namespace TutorDemand.Data.Base
{
    public class BaseDAO<T>
        where T : class
    {
        protected NET1704_221_5_TutorDemandContext _context;
        protected DbSet<T> _dbSet;

        public BaseDAO()
        {
            _context = new NET1704_221_5_TutorDemandContext();
            _dbSet = _context.Set<T>();
        }

        public List<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public void Create(T entity)
        {
            _dbSet.Add(entity);
        }

        public async Task CreateAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
            var tracker = _context.Attach(entity);
            tracker.State = EntityState.Modified;
        }

        public async Task UpdateAsync(T entity)
        {
            var tracker = _context.Attach(entity);
            tracker.State = EntityState.Modified;

            await Task.CompletedTask;
        }

        public void Remove(T entity)
        {
            // Provide access to tracking information and operations
            if (_context.Entry(entity).State == EntityState.Detached) // entity is not tracking by context
            {
                // Ensure that entity is being track by context
                _context.Attach(entity); // Change entity's state to "Deleted"
            }

            _dbSet.Remove(entity);
        }

        public async Task RemoveAsync(T entity)
        {
            _dbSet.Remove(entity);

            await Task.CompletedTask;
        }

        public T? GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public T? GetById(string code)
        {
            return _dbSet.Find(code);
        }

        public async Task<T?> GetByIdAsync(string code)
        {
            return await _dbSet.FindAsync(code);
        }

        public T? GetById(Guid code)
        {
            return _dbSet.Find(code);
        }

        public async Task<T?> GetByIdAsync(Guid code)
        {
            return await _dbSet.FindAsync(code);
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public IEnumerable<T> GetWithCondition(
            Expression<Func<T, bool>> filter = null!,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null!,
            string includeProperties = ""
        )
        {
            IQueryable<T> query = _dbSet;

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

        public async Task<IEnumerable<T>> GetWithConditionAsync(
            Expression<Func<T, bool>> filter = null!,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null!,
            string includeProperties = ""
        )
        {
            IQueryable<T> query = _dbSet;

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
                return await orderBy(query).ToListAsync();
            else
                return await query.ToListAsync();
        }

        public T? FindOne(Expression<Func<T, bool>> expression)
        {
            return _dbSet.Where(expression).FirstOrDefault();
        }

        public async Task<T?> FindOneAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.Where(expression).FirstOrDefaultAsync();
        }

        public bool Exist(Expression<Func<T, bool>> expression)
        {
            return _dbSet.Where(expression).Any();
        }

        public async Task<bool> ExistAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.Where(expression).AnyAsync();
        }
    }
}