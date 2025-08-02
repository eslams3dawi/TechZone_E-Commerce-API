using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechZone.DAL.Models;
using TechZone.DAL.Repository.GenericRepo;

namespace TechZone.DAL.Repository.ShoppingCartRepo
{
    public interface IShoppingCartRepository : IGenericRepository<ShoppingCart>
    {
        public Task<List<ShoppingCart>> GetByUserIdWithProducts(string userId);
        public Task<ShoppingCart?> GetCartItemByUserAndProduct(string userId, int productId);
        public Task<IEnumerable<ShoppingCart>> GetByUserId(string userId);
        public Task<ShoppingCart?> GetByProductIdForUserCart(string userId, int productId);
    }
}
