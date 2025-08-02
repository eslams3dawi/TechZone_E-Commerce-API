using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechZone.BLL.DTOs.AccountDTOs
{
    public class RegisterDTO
    {
        [EmailAddress(ErrorMessage = "Invalid Email Format")]
        [Required(ErrorMessage = "Email Field Is Required")]
        public string Email { get; set; }

        [MaxLength(35, ErrorMessage = "The Field Exceeded The Max Length")]
        [Required(ErrorMessage = "First Name Field Is Required")]
        public string FirstName { get; set; }

        [MaxLength(35, ErrorMessage = "The Field Exceeded The Max Length")]
        [Required(ErrorMessage = "Last Name Field Is Required")]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "BirthDate Field Is Required")]
        public DateTime BirthDate { get; set; }

        [Phone]
        [Required(ErrorMessage = "Phone Number Field Is Required")]
        public string PhoneNumber { get; set; }


        [Required(ErrorMessage = "Password Field Is Required")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Compare("Password", ErrorMessage = "Passwords Do Not Match")]
        [Required]
        public string PasswordConfirmed { get; set; }
    }
}

