using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechZone.BLL.DTOs.ShoppingCartDTOs
{
    public class ShoppingCartAddDTO
    {
        public int ProductId { get; set; }
        public string UserId { get; set; }

        [Required(ErrorMessage = "Count field is required")]
        [Range(1, 100)]
        public int Count { get; set; }
    }
}
