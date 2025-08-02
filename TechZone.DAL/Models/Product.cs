using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechZone.DAL.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Description { get; set; }
        public decimal ListPrice { get; set; }
        public decimal Price { get; set; } //discounted
        public string? ImgUrl { get; set; }
        public int Stock { get; set; }
        public bool IsDeleted { get; set; }

        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}
