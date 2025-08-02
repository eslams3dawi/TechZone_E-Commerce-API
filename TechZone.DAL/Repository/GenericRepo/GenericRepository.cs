using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TechZone.DAL.Database;
using TechZone.DAL.Models;

namespace TechZone.DAL.Repository.GenericRepo
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly TechZoneContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(TechZoneContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public async Task Add(T entity)
        {
            _context.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(T entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<T> GetById(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task Update(T entity)
        {
            await _context.SaveChangesAsync();
        }

        public async Task SoftDelete(T entity)
        {
            var isDeletedProperty = typeof(T).GetProperty("IsDeleted");

            if(isDeletedProperty == null)
                throw new NullReferenceException();

            isDeletedProperty.SetValue(entity, true);
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _dbSet.CountAsync();
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task DeleteRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
            await _context.SaveChangesAsync();
        }
        public async Task<T?> GetFirstOrDefault(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            IQueryable<T> query = _dbSet;

            if (include != null)
                query = include(query);

            return await query.FirstOrDefaultAsync(expression);
        }
    }
}
