using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechZone.DAL.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }

        public int OrderHeaderId { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string Brand { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice => Price * Count;

        public virtual Product Product { get; set; }
        public virtual OrderHeader OrderHeader { get; set; }
    }
}
