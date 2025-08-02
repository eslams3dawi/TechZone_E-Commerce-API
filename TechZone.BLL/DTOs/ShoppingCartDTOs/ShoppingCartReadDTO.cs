using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechZone.BLL.DTOs.ShoppingCartDTOs
{
    public class ShoppingCartReadDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ProductId { get; set; }  
            public string ProductName { get; set; }
            public decimal ProductPrice { get; set; }
            public string ProductImgUrl { get; set; }
        public int Count { get; set; }
    }
}
