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
    public class OrderHeaderConfigurations : IEntityTypeConfiguration<OrderHeader>
    {
        public void Configure(EntityTypeBuilder<OrderHeader> builder)
        {
            builder.ToTable("OrderHeaders");
            builder.HasKey(o => o.Id);

            builder.Property(o => o.OrderDate);

            builder.Property(o => o.ShippingDate);

            builder.Property(o => o.OrderStatus)
                .IsRequired();

            builder.Property(o => o.FirstName)
                .HasMaxLength(35)
                .IsRequired();

            builder.Property(o => o.LastName)
                .HasMaxLength(35)
                .IsRequired();

            builder.Property(o => o.PhoneNumber)
                .HasMaxLength(11)
                .IsRequired();

            builder.Property(o => o.StreetAddress)
                .HasMaxLength(80)
                .IsRequired();

            builder.Property(o => o.City)
                .HasMaxLength(80)
                .IsRequired();

            builder.HasOne(o => o.ApplicationUser)
                .WithMany(o => o.Orders)
                .HasForeignKey(o => o.ApplicationUserId);
        }
    }
}
