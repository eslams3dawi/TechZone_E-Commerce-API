using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechZone.BLL.DTOs.CategoryDTO;
using TechZone.BLL.DTOs.CategoryDTOs;
using TechZone.BLL.DTOs.ProductDTO;
using TechZone.BLL.Wrappers;
using TechZone.DAL.Models;
using TechZone.DAL.Repository.CategoryRepo;

namespace TechZone.BLL.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private const string _cache_Key = "Cache_Category";
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper, IMemoryCache memoryCache)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _memoryCache = memoryCache;
        }

        public async Task<int> AddCategory(CategoryAddDTO categoryAddDTO)
        {
            var categoryModel = _mapper.Map<Category>(categoryAddDTO);

            await _categoryRepository.Add(categoryModel);
            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions()
            {
                SlidingExpiration = TimeSpan.FromDays(3)
            };
            _memoryCache.Set($"{_cache_Key}: {categoryModel.CategoryId}", categoryModel, options);
            RemoveCacheList();
            return categoryModel.CategoryId;
        }

        public async Task DeleteCategory(int id)
        {
            var categoryModel = await _categoryRepository.GetById(id);
            await _categoryRepository.Delete(categoryModel);

            RemoveCacheCategory(id);
        }

        public async Task<Result<IEnumerable<ProductReadDTO>>> GetAllProductsUnderSpecificCategory(int categoryId)
        {
            var products = await _categoryRepository.GetAllProductsUnderSpecificCategory(categoryId);

            //products == null → there is no category with this id
            //!products.Any → empty list of products under this category
            if (products == null || !products.Any())
                return Result<IEnumerable<ProductReadDTO>>.Failure("Target category has no products", null, ActionCode.NotFound);

            var productsDTO = _mapper.Map<IEnumerable<ProductReadDTO>>(products);

            return Result<IEnumerable<ProductReadDTO>>.Success(productsDTO, "Products Retrieved Successfully");
        }

        public async Task<Result<IEnumerable<CategoryDTO>>> GetCategories()
        {
            if(!_memoryCache.TryGetValue(_cache_Key, out IEnumerable<CategoryDTO> categoryDTOs))
            {
                var categoryModels = await _categoryRepository.GetAll();
                if (categoryModels == null || !categoryModels.Any())
                    return Result<IEnumerable<CategoryDTO>>.Failure("No categories found", null, ActionCode.NotFound);

                categoryDTOs = _mapper.Map<IEnumerable<CategoryDTO>>(categoryModels);

                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions()
                {
                    SlidingExpiration = TimeSpan.FromDays(3)
                };
                _memoryCache.Set(_cache_Key, categoryDTOs, options);
            }
                return Result<IEnumerable<CategoryDTO>>.Success(categoryDTOs, "Categories Retrieved Successfully");
        }

        public async Task<Result<CategoryDTO>> GetCategoryById(int id)
        {
            if(!_memoryCache.TryGetValue($"{_cache_Key}: {id}", out CategoryDTO categoryDTO))
            {
                var categoryModel = await _categoryRepository.GetById(id);

                if (categoryModel == null)
                    return Result<CategoryDTO>.Failure($"Category with id: {id} not found", null, ActionCode.NotFound);

                categoryDTO = _mapper.Map<CategoryDTO>(categoryModel);

                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions()
                {
                    SlidingExpiration = TimeSpan.FromDays(3)
                };

                _memoryCache.Set($"{_cache_Key}: {id}", categoryDTO, options);
            }
            return Result<CategoryDTO>.Success(categoryDTO);
        }

        public async Task UpdateCategory(CategoryDTO categoryDTO)
        {
            var categoryModel = await _categoryRepository.GetById(categoryDTO.CategoryId);
            _mapper.Map(categoryDTO, categoryModel);
            await _categoryRepository.Update(categoryModel);

            RemoveCacheCategory(categoryDTO.CategoryId);
        }

        private void RemoveCacheCategory(int categoryId)
        {
            if(_memoryCache.TryGetValue($"{_cache_Key}: {categoryId}", out _))
                _memoryCache.Remove($"{_cache_Key}: {categoryId}");

            if(_memoryCache.TryGetValue(_cache_Key, out IEnumerable<CategoryDTO> categoryDTOs))
                if (categoryDTOs.Any(c => c.CategoryId == categoryId))
                    _memoryCache.Remove(_cache_Key);
        }
        private void RemoveCacheList()
        {
            _memoryCache.Remove(_cache_Key);
        }
    }
}
