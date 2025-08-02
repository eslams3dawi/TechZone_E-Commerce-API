using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TechZone.DAL.Repository.GenericRepo
{
    public interface IGenericRepository<T>
    {
        public Task<IEnumerable<T>> GetAll();
        public Task<T> GetById(int id);
        public Task Update(T entity);
        public Task Add(T entity);
        public Task Delete(T entity);
        public Task DeleteRange(IEnumerable<T> entities);
        public Task SoftDelete(T entity);
        public Task<int> GetTotalCountAsync();


        //Expression is the condition (bool)
        //Func<IQueryable<T>, IIncludableQueryable<T, object>> : to pass query
            //IIncludableQueryable<T, object>  = null : is the default value for this case, can pass include
                 //Ex. include: q => q.Include(o => o.OrderDetails)  
        public Task<T?> GetFirstOrDefault(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);
    }
}
