using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechZone.DAL.Models;

namespace TechZone.BLL.DTOs.OrderDTOs
{
    public class OrderDetailReadDTO
    {
        public int Id { get; set; }
        public int OrderHeaderId { get; set; }

        public int ProductId { get; set; }
        public string ProductName { get; set; }//mapped
        public int Count { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice => Price * Count;
    }
}
