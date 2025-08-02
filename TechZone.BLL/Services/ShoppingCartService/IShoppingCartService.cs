using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechZone.BLL.DTOs.ShoppingCartDTOs;
using TechZone.BLL.Wrappers;
using TechZone.DAL.Models;

namespace TechZone.BLL.Services.ShoppingCartService
{
    public interface IShoppingCartService
    {
        public Task<Result<IEnumerable<ShoppingCartReadDTO>>> GetCart(string userId);
        public Task AddToCart(ShoppingCartAddDTO shoppingCartAddDTO);
        public Task UpdateProductCountInCart(ShoppingCartUpdateDTO shoppingCartUpdateDTO);
        public Task DeleteProductInCart(string userId, int productId);
        public Task ClearCart(string userId);
    }
}
