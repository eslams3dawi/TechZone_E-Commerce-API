using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechZone.DAL.Models;

namespace TechZone.DAL.Configurations
{
    public class UserConfigurations : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(u => u.FirstName)
                .HasMaxLength(35);
            builder.Property(u => u.LastName)
                .HasMaxLength(35);
            builder.Property(u => u.StreetAddress)
                .HasMaxLength(80);
            builder.Property(u => u.City)
                .HasMaxLength(80);
        }
    }
}
