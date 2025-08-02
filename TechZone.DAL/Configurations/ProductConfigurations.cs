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
    public class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");
            builder.HasKey(p => p.ProductId);

            builder.Property(p => p.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(p => p.Brand)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.Description)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(p => p.ListPrice)
                .HasPrecision(8, 2) 
                .IsRequired();

            builder.Property(p => p.Price)
                .HasPrecision(8, 2)
                .IsRequired();

            builder.Property(p => p.ImgUrl)
                .IsRequired(false);

            builder.Property(p => p.Stock)
                .IsRequired();

            builder.HasOne(p => p.Category)
                .WithMany(p => p.Products)
                .HasForeignKey(p => p.CategoryId);
        }
    }
}
