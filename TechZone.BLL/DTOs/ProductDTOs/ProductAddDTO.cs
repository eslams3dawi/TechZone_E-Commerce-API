using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechZone.BLL.DTOs.ProductDTO
{
    public class ProductAddDTO
    {
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Description { get; set; }
        public decimal ListPrice { get; set; }
        public decimal Price { get; set; }
        public string? ImgUrl { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; }

    }
}
