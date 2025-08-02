using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechZone.DAL.Models;
using TechZone.DAL.Repository.GenericRepo;

namespace TechZone.DAL.Repository.CategoryRepo
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        public Task<IEnumerable<Product>> GetAllProductsUnderSpecificCategory(int categoryId);
    }
}
