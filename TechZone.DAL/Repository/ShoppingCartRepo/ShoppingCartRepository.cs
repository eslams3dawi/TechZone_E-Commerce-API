using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechZone.DAL.Database;
using TechZone.DAL.Models;
using TechZone.DAL.Repository.GenericRepo;

namespace TechZone.DAL.Repository.ShoppingCartRepo
{
    public class ShoppingCartRepository : GenericRepository<ShoppingCart>, IShoppingCartRepository
    {
        private readonly TechZoneContext _context;

        public ShoppingCartRepository(TechZoneContext context): base(context)
        {
            _context = context;
        }

        public async Task<ShoppingCart?> GetByProductIdForUserCart(string userId, int productId)
        {
            return await _context.ShoppingCarts
                .FirstOrDefaultAsync(s => s.UserId == userId && s.ProductId == productId);
        }

        public async Task<IEnumerable<ShoppingCart>> GetByUserId(string userId)
        {
            return await _context.ShoppingCarts
                .Where(s => s.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<ShoppingCart>> GetByUserIdWithProducts(string userId)
        {
            return await _context.ShoppingCarts
                   .Where(s => s.UserId == userId)
                   .Include(s => s.Product)
                   .ToListAsync();
        }

        public async Task<ShoppingCart?> GetCartItemByUserAndProduct(string userId, int productId)
        {
            return await _context.ShoppingCarts
                .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);
        }
    }
}
