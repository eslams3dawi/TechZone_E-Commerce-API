using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechZone.DAL.Database;
using TechZone.DAL.Models;
using TechZone.DAL.Repository.GenericRepo;

namespace TechZone.DAL.Repository.ProductRepo
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly TechZoneContext _context;

        public ProductRepository(TechZoneContext context): base(context)
        {
            _context = context;
        }
        public async Task<Product?> GetProductWithCategoryName(int id)
        {
            return await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.ProductId == id);
        }
        public async Task<IEnumerable<Product>> GetAllPagination(string filter, string search, int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                query = query
                .Where(q => q.Brand == filter);
            }
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query
                .Where(q => q.Name.Contains(search));
            }
            var products = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return products;
        }
    }
}
