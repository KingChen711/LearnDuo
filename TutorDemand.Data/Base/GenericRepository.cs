using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TutorDemand.Data.Entities;

namespace TutorDemand.Data.Base
{
    public class GenericRepository<T>
        where T : class
    {
        protected NET1704_221_5_TutorDemandContext _context;

        public async Task<T?> GetByIdAsync(Guid code)
        {
            return await _context.Set<T>().FindAsync(code);
        }

        public void PrepareCreate(T entity)
        {
            _context.Add(entity);
        }

        public async Task PrepareCreateAsync(T entity)
        {
            await _context.AddAsync(entity);
        }

        public GenericRepository()
        {
            _context ??= new NET1704_221_5_TutorDemandContext();
        }

        #region Separating asign entity and save operators

        public GenericRepository(NET1704_221_5_TutorDemandContext context)
        {
            _context = context;
        }

        public void PrepareUpdate(T entity)
        {
            var tracker = _context.Attach(entity);
            tracker.State = EntityState.Modified;
        }

        public void PrepareRemove(T entity)
        {
            _context.Remove(entity);
        }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        #endregion Separating asign entity and save operators


        public List<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public async Task<List<T>> GetAllAsync(bool trackChanges = true)
        {
            return trackChanges
                ? await _context.Set<T>().ToListAsync()
                : await _context.Set<T>().AsNoTracking().ToListAsync();
        }

        public void Create(T entity)
        {
            _context.Add(entity);
            _context.SaveChanges();
        }

        public async Task<int> CreateAsync(T entity)
        {
            try
            {
                await _context.AddAsync(entity);
                var rs = await _context.SaveChangesAsync();
                return rs;
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e.Message);
                return -1;
            }
        }

        public void Update(T entity)
        {
            var tracker = _context.Attach(entity);
            tracker.State = EntityState.Modified;
            _context.SaveChanges();
        }

        public async Task<int> UpdateAsync(T entity)
        {
            var tracker = _context.Attach(entity);
            tracker.State = EntityState.Modified;
            return await _context.SaveChangesAsync();
        }

        public bool Remove(T entity)
        {
            _context.Remove(entity);
            _context.SaveChanges();
            return true;
        }

        public async Task<bool> RemoveAsync(T entity)
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public T? GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public T? GetById(string code)
        {
            return _context.Set<T>().Find(code);
        }

        public async Task<T?> GetByIdAsync(string code)
        {
            return await _context.Set<T>().FindAsync(code);
        }

        public T? GetById(Guid code)
        {
            return _context.Set<T>().Find(code);
        }

        public List<T> GetWithCondition(Expression<Func<T, bool>> condition)
        {
            return _context.Set<T>().Where(condition).ToList();
        }

        public async Task<List<T>> GetWithConditionAsync(Expression<Func<T, bool>> condition)
        {
            return await _context.Set<T>().Where(condition).ToListAsync();
        }

        public T? GetOneWithCondition(Expression<Func<T, bool>> condition)
        {
            return _context.Set<T>().Where(condition).FirstOrDefault();
        }

        public async Task<T?> GetOneWithConditionAsync(
            Expression<Func<T, bool>> condition,
            bool trackChanges = true
        )
        {
            return trackChanges
                ? await _context.Set<T>().Where(condition).FirstOrDefaultAsync()
                : await _context.Set<T>().Where(condition).AsNoTracking().FirstOrDefaultAsync();
        }

        public bool Exist(Expression<Func<T, bool>> condition)
        {
            return _context.Set<T>().Any(condition);
        }

        public async Task<bool> ExistAsync(Expression<Func<T, bool>> condition)
        {
            return await _context.Set<T>().AnyAsync(condition);
        }

        public IQueryable<T> GetQueryable(bool trackChanges = true) =>
            trackChanges
                ? _context.Set<T>().AsQueryable()
                : _context.Set<T>().AsNoTracking().AsQueryable();
    }
}
