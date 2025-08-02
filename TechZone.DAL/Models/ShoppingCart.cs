using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechZone.DAL.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ProductId { get; set; }
        public string Brand { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
        public virtual Product Product { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
