using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechZone.BLL.DTOs.CategoryDTOs
{
    public class CategoryAddDTO
    {
        [Required(ErrorMessage = "Category name field is required")]
        [MaxLength(50, ErrorMessage = "Category name exceeded the maximum length")]
        public string Name { get; set; }
    }
}
