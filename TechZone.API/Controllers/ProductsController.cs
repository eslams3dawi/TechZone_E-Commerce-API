using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechZone.BLL.DTOs.ProductDTO;
using TechZone.BLL.Services.ProductServices;

namespace TechZone.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> GetProducts(string filter = null, string search = null, int pageNumber = 1, int pageSize = 10)
        {
            var products = await _productService.GetAllPagination(filter, search, pageNumber, pageSize);

            return Ok(products);
        }

        [AllowAnonymous]
        [HttpGet("{id}/details")]
        public async Task<ActionResult> GetProductWithCategoryNameAsync(int id)
        {
            var product = await _productService.GetProductWithCategoryNameAsync(id);

            return Ok(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> AddProduct(ProductAddDTO productAddDTO)
        {
            var productId = await _productService.AddAsync(productAddDTO);
            return CreatedAtAction(nameof(GetProductWithCategoryNameAsync), new { id = productId }, new {Message = "Product Created Successfully"});
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> SoftDeleteProduct(int id)
        {
            await _productService.SoftDeleteAsync(id);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("{id}")]
        public async Task<ActionResult> UpdateProductById(int id, ProductUpdateDTO productUpdateDTO)
        {
            if (id != productUpdateDTO.ProductId)
                return BadRequest();

            await _productService.UpdateAsync(productUpdateDTO);
            return NoContent();
        }
    }
}
