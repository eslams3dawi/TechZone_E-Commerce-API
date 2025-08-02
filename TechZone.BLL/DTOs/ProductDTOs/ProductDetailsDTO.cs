using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechZone.BLL.DTOs.ProductDTO
{
    public class ProductDetailsDTO
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string? ImgUrl { get; set; }
        public int Stock { get; set; }
        public string CategoryName { get; set; }
    }
}
