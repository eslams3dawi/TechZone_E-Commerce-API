using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechZone.BLL.DTOs.CategoryDTO;
using TechZone.BLL.DTOs.CategoryDTOs;
using TechZone.BLL.DTOs.ProductDTO;
using TechZone.BLL.Services.CategoryService;
using TechZone.BLL.Wrappers;

namespace TechZone.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> AddCategory(CategoryAddDTO categoryAddDTO)
        {
            var categoryId = await _categoryService.AddCategory(categoryAddDTO); //void

            return CreatedAtAction(nameof(GetCategoryById), new { id = categoryId }, new { Message = "Category Created Successfully" });
        }

        [AllowAnonymous]
        [HttpGet("{id}/Products")]
        public async Task<ActionResult<Result<IEnumerable<ProductReadDTO>>>> GetAllProductsUnderSpecificCategory(int id)
        {
            var products = await _categoryService.GetAllProductsUnderSpecificCategory(id);
            return products;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetCategoryById(int id)
        {
            var categoryModel = await _categoryService.GetCategoryById(id);
            return Ok(categoryModel);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet]
        public async Task<ActionResult> GetCategories()
        {
            var categoryModels = await _categoryService.GetCategories();
            return Ok(categoryModels);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            await _categoryService.DeleteCategory(id);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCategory(int id, CategoryDTO categoryDTO)
        {
            if (id != categoryDTO.CategoryId)
                return BadRequest();

            await _categoryService.UpdateCategory(categoryDTO);
            return NoContent();
        }
    }
}
