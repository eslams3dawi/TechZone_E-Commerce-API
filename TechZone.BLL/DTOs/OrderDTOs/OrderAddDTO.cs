using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechZone.DAL.Enums.Order;
using TechZone.DAL.Models;

namespace TechZone.BLL.DTOs.OrderDTOs
{
    public class OrderAddDTO
    {
        [Required]
        public string ApplicationUserId { get; set; }

        [MaxLength(35, ErrorMessage = "The Field Exceeded The Max Length")]
        [Required(ErrorMessage = "First Name Field Is Required")]
        public string FirstName { get; set; }

        [MaxLength(35, ErrorMessage = "The Field Exceeded The Max Length")]
        [Required(ErrorMessage = "Last Name Field Is Required")]
        public string LastName { get; set; }
        [Phone]
        [Required(ErrorMessage = "Phone Number Field Is Required")]
        public string PhoneNumber { get; set; }

        [MaxLength(80, ErrorMessage = "The Field Exceeded The Max Length")]
        [Required(ErrorMessage = "Street Address Field Is Required")]
        public string StreetAddress { get; set; }

        [MaxLength(80, ErrorMessage = "The Field Exceeded The Max Length")]
        [Required(ErrorMessage = "City Field Is Required")]
        public string City { get; set; }
    }
}
