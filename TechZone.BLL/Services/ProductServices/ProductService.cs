using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechZone.API.Middleware.CustomExceptions;
using TechZone.BLL.DTOs.ProductDTO;
using TechZone.BLL.Services.CategoryService;
using TechZone.BLL.Wrappers;
using TechZone.DAL.Models;
using TechZone.DAL.Repository.ProductRepo;

namespace TechZone.BLL.Services.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, ICategoryService categoryService ,IMapper mapper)
        {
            _productRepository = productRepository;
            _categoryService = categoryService;
            _mapper = mapper;
        }
        public async Task AddAsync(ProductAddDTO productAddDTO)
        {
            //clear cart items after order creation
            var existingCategory = await _categoryService.GetCategoryById(productAddDTO.CategoryId);
            if (existingCategory.Data != null)
            {
                var productModel = _mapper.Map<ProductAddDTO, Product>(productAddDTO);
                await _productRepository.Add(productModel);
                return;
            }
            throw new BadRequestException("Invalid categoryId, categoryId not found");
        }   

        public async Task DeleteAsync(int id)
        {
            var productModel = await _productRepository.GetById(id);

            await _productRepository.Delete(productModel);
        }

        public async Task<Result<PagedResult<ProductReadDTO>>> GetAllPagination(string filter, string search, int pageNumber = 1, int pageSize = 10)
        {
            //1. Paginated data
            var productModels = await _productRepository.GetAllPagination(filter, search, pageNumber, pageSize);
            if (productModels == null)
                return Result<PagedResult<ProductReadDTO>>.Failure("No products found", null, ActionCode.NotFound);
            //2. total count of items
            var totalItems = await _productRepository.GetTotalCountAsync();

            var productDTOs = _mapper.Map<IEnumerable<ProductReadDTO>>(productModels);

            //3. create paged result
            var PagedResult = new PagedResult<ProductReadDTO>()
            {
                Items = productDTOs,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = totalItems
            };

            //4. Wrap with Result<T>
            return Result<PagedResult<ProductReadDTO>>.Success(PagedResult);
        }

        public async Task<Result<ProductDetailsDTO>> GetProductWithCategoryNameAsync(int id)
        {
            var productModel = await _productRepository.GetProductWithCategoryName(id);
            //ToDo: Null
            if (productModel == null)
                return Result<ProductDetailsDTO>.Failure($"Product with id: {id} not found", null, ActionCode.NullReference);

            var productDto = _mapper.Map<ProductDetailsDTO>(productModel);
            return Result<ProductDetailsDTO>.Success(productDto);
        }

        public async Task SoftDeleteAsync(int id)
        {
            var entity = await _productRepository.GetById(id);
            //ToDo: Null
            if (entity == null)
                throw new KeyNotFoundException();

            await _productRepository.SoftDelete(entity);
        }

        public async Task UpdateAsync(ProductUpdateDTO productUpdateDTO)
        {
            var productModel = await _productRepository.GetById(productUpdateDTO.ProductId);
            _mapper.Map(productUpdateDTO, productModel);
            await _productRepository.Update(productModel);
        }
    }
}
