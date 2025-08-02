using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechZone.DAL.Database;
using TechZone.DAL.Models;
using TechZone.DAL.Repository.GenericRepo;

namespace TechZone.DAL.Repository.CategoryRepo
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly TechZoneContext _context;

        public CategoryRepository(TechZoneContext context): base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Product>> GetAllProductsUnderSpecificCategory(int categoryId)
        {
            var category = await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.CategoryId == categoryId);
                
            return category.Products.ToList();
        }
    }
}
