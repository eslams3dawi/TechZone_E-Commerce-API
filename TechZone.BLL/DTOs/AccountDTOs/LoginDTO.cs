using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechZone.BLL.DTOs.AccountDTOs
{
    public class LoginDTO
    {
        [EmailAddress(ErrorMessage = "Invalid Email Format")]
        [Required(ErrorMessage = "Email Field Is Required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password Field Is Required")]
        public string Password { get; set; }
    }
}
