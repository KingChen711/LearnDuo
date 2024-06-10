using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TutorDemand.Data.Entities;

namespace TutorDemand.Data.Base
{
    public class GenericRepository<T> where T : class
    {
        protected NET1704_221_5_TutorDemandContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository()
        {
            _context ??= new NET1704_221_5_TutorDemandContext();
            _dbSet = _context.Set<T>();
        }

        #region Separating asign entity and save operators

        public GenericRepository(NET1704_221_5_TutorDemandContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public void PrepareCreate(T entity)
        {
            _dbSet.Add(entity);
        }

        public void PrepareUpdate(T entity)
        {
            var tracker = _context.Attach(entity);
            tracker.State = EntityState.Modified;
        }

        public void PrepareRemove(T entity)
        {
            _dbSet.Remove(entity);
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
            return _dbSet.ToList();
        }

        public async Task<List<T>> GetAllAsync(bool trackChanges = true)
        {
            return trackChanges ? await _dbSet.ToListAsync() : await _dbSet.AsNoTracking().ToListAsync();
        }

        public void Create(T entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
        }

        public async Task<int> CreateAsync(T entity)
        {
            try
            {
                _dbSet.Add(entity);
                var a= await _context.SaveChangesAsync();
                Console.WriteLine(a);
                return a;
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
            _dbSet.Remove(entity);
            _context.SaveChanges();
            return true;
        }

        public async Task<bool> RemoveAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
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

        public List<T> GetWithCondition(Expression<Func<T, bool>> condition)
        {
            return _dbSet.Where(condition).ToList();
        }

        public async Task<List<T>> GetWithConditionAsync(Expression<Func<T, bool>> condition)
        {
            return await _dbSet.Where(condition).ToListAsync();
        }

        public T? GetOneWithCondition(Expression<Func<T, bool>> condition)
        {
            return _dbSet.Where(condition).FirstOrDefault();
        }

        public async Task<T?> GetOneWithConditionAsync(Expression<Func<T, bool>> condition, bool trackChanges = true)
        {
            return trackChanges
                ? await _dbSet.Where(condition).FirstOrDefaultAsync()
                : await _dbSet.Where(condition).AsNoTracking().FirstOrDefaultAsync();
        }

        public bool Exist(Expression<Func<T, bool>> condition)
        {
            return _dbSet.Any(condition);
        }

        public async Task<bool> ExistAsync(Expression<Func<T, bool>> condition)
        {
            return await _dbSet.AnyAsync(condition);
        }

        public IQueryable<T> GetQueryable(bool trackChanges)
            => trackChanges ? _dbSet.AsQueryable() : _dbSet.AsNoTracking().AsQueryable();
    }
}