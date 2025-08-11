using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechZone.BLL.DTOs.ProductDTO;
using TechZone.BLL.Wrappers;
using TechZone.DAL.Models;

namespace TechZone.BLL.Services.ProductServices
{
    public interface IProductService
    {
        public Task<int> AddAsync(ProductAddDTO productAddDTO);
        public Task<Result<PagedResult<ProductReadDTO>>> GetAllPagination(string filter, string search, int pageNumber = 1, int pageSize = 10);      
        public Task<Result<ProductDetailsDTO>> GetProductWithCategoryNameAsync(int id);
        public Task UpdateAsync(ProductUpdateDTO productUpdateDTO);
        public Task DeleteAsync(int id);
        public Task SoftDeleteAsync(int id);
    }
}
