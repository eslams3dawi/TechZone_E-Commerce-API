using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechZone.BLL.DTOs.ShoppingCartDTOs;
using TechZone.BLL.Services.ShoppingCartService;
using TechZone.BLL.Wrappers;

namespace TechZone.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartsController : ControllerBase
    {
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartsController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        [Authorize(Roles = "Admin, User")]
        [HttpPost("cart/products")]
        public async Task<ActionResult> AddToCart(ShoppingCartAddDTO shoppingCartAddDTO)
        {
            var cartItemId = await _shoppingCartService.AddToCart(shoppingCartAddDTO);

            if(cartItemId == 0)
                return Ok($"Item Count Increased by {shoppingCartAddDTO.Count}");

            return CreatedAtAction(nameof(GetUserCart), new { id = cartItemId }, new { Message = "Item Add To Cart Successfully" });
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet("users/{userId}/cart")]
        public async Task<ActionResult<Result<IEnumerable<ShoppingCartReadDTO>>>> GetUserCart(string userId)
        {
            var userCart = await _shoppingCartService.GetCart(userId);

            return Ok(userCart);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpDelete("users/{userId}/cart")]
        public async Task<ActionResult> ClearCart(string userId)
        {
            await _shoppingCartService.ClearCart(userId);

            return NoContent();
        }

        [Authorize(Roles = "Admin, User")]
        [HttpDelete("users/{userId}/cart/products/{productId}")]
        public async Task<ActionResult> DeleteProductInCart(string userId, int productId)
        {
            await _shoppingCartService.DeleteProductInCart(userId, productId);

            return NoContent();
        }

        [Authorize(Roles = "Admin, User")]
        [HttpPut("cart/products/count")]
        public async Task<ActionResult> UpdateProductCountInCart(ShoppingCartUpdateDTO shoppingCartUpdateDTO)
        {
            await _shoppingCartService.UpdateProductCountInCart(shoppingCartUpdateDTO);

            return NoContent();
        }
    }
}
