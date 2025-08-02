using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechZone.DAL.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public DateTime BirthDate { get; set; }

        public virtual ICollection<OrderHeader> Orders { get; set; }
    }
}
