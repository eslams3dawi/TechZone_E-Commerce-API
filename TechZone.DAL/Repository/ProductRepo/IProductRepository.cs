using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechZone.DAL.Models;
using TechZone.DAL.Repository.GenericRepo;

namespace TechZone.DAL.Repository.ProductRepo
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        public Task<Product?> GetProductWithCategoryName(int id);
        public Task<IEnumerable<Product>> GetAllPagination(string filter, string search, int pageNumber = 1, int pageSize = 10);
    }
}
