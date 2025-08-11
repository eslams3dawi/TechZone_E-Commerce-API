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

namespace TechZone.BLL.Services.CategoryService
{
    public interface ICategoryService
    {
        public Task<Result<IEnumerable<CategoryDTO>>> GetCategories();
        public Task<Result<CategoryDTO>> GetCategoryById(int id);
        public Task<int> AddCategory(CategoryAddDTO categoryAddDTO);
        public Task UpdateCategory(CategoryDTO categoryDTO);
        public Task DeleteCategory(int id);
        public Task<Result<IEnumerable<ProductReadDTO>>> GetAllProductsUnderSpecificCategory(int categoryId);
        
    }
}
