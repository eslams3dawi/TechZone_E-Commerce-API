using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechZone.API.Middleware.CustomExceptions;
using TechZone.BLL.DTOs.ShoppingCartDTOs;
using TechZone.BLL.Wrappers;
using TechZone.DAL.Models;
using TechZone.DAL.Repository.ProductRepo;
using TechZone.DAL.Repository.ShoppingCartRepo;

namespace TechZone.BLL.Services.ShoppingCartService
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ShoppingCartService(IShoppingCartRepository shoppingCartRepository, IProductRepository productRepository, IMapper mapper)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }
        public async Task AddToCart(ShoppingCartAddDTO shoppingCartAddDTO)
        {
            var existingItem = await _shoppingCartRepository.
                GetCartItemByUserAndProduct(shoppingCartAddDTO.UserId, shoppingCartAddDTO.ProductId);

            if(existingItem != null) 
            {
                //if exist, increase only the count
                existingItem.Count += shoppingCartAddDTO.Count;
                await _shoppingCartRepository.Update(existingItem);
            }
            else if(shoppingCartAddDTO.Count < 1)
            {
                throw new InvalidOperationException("Invalid count input");
            }
            else
            {
                //else: create one for this item
                var cartItemModel = _mapper.Map<ShoppingCart>(shoppingCartAddDTO);

                var product = await _productRepository.GetById(shoppingCartAddDTO.ProductId);
                cartItemModel.Price = product.Price;
                cartItemModel.Brand = product.Brand;
                await _shoppingCartRepository.Add(cartItemModel);
            }
        }

        public async Task ClearCart(string userId)
        {
            var cartItems = await _shoppingCartRepository.GetByUserId(userId);

            await _shoppingCartRepository.DeleteRange(cartItems);
        }

        public async Task DeleteProductInCart(string userId, int productId)
        {
            var cartItem = await _shoppingCartRepository.GetByProductIdForUserCart(userId, productId);

            if (cartItem == null)
                throw new BadRequestException($"Product Id: {productId} not found in this cart");

            await _shoppingCartRepository.Delete(cartItem);
        }

        public async Task<Result<IEnumerable<ShoppingCartReadDTO>>> GetCart(string userId)
        {
            var shoppingCart = await _shoppingCartRepository.GetByUserIdWithProducts(userId);

            var cartDTO = _mapper.Map<List<ShoppingCartReadDTO>>(shoppingCart);
            return Result<IEnumerable<ShoppingCartReadDTO>>.Success(cartDTO);
        }

        public async Task UpdateProductCountInCart(ShoppingCartUpdateDTO shoppingCartUpdateDTO)
        {
            var cartItem = await _shoppingCartRepository.GetCartItemByUserAndProduct(shoppingCartUpdateDTO.UserId, shoppingCartUpdateDTO.ProductId);
            if (cartItem == null)
                throw new BadRequestException($"Product with id: {shoppingCartUpdateDTO.ProductId} is not exist in this cart");

            if (shoppingCartUpdateDTO.Count < 1)
                throw new InvalidOperationException("Invalid count input");

            cartItem.Count = shoppingCartUpdateDTO.Count;
            await _shoppingCartRepository.Update(cartItem);
        }
    }
}
