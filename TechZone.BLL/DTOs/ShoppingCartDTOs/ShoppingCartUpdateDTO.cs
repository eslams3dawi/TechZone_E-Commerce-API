using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechZone.BLL.DTOs.ShoppingCartDTOs
{
    public class ShoppingCartUpdateDTO
    {
        public string UserId { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
    }
}
