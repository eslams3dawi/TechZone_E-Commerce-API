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
    public class ShoppingCartConfigurations : IEntityTypeConfiguration<ShoppingCart>
    {
        public void Configure(EntityTypeBuilder<ShoppingCart> builder)
        {
            builder.ToTable("ShoppingCarts");
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Count)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(s => s.Price)
                .HasPrecision(8, 2)
                .IsRequired();

            builder.HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.UserId);

            builder.HasOne(s => s.Product)
                .WithMany()
                .HasForeignKey(s => s.ProductId);
        }
    }
}
