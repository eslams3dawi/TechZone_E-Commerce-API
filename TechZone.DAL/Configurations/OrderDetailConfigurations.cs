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
    public class OrderDetailConfigurations : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.ToTable("OrderDetails");
            builder.HasKey(o => o.Id);

            builder.Property(o => o.Count)
                .IsRequired();

            builder.Property(o => o.Price)
                .HasPrecision(8, 2)
                .IsRequired();

            builder.HasOne(o => o.OrderHeader)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(o => o.OrderHeaderId);

            builder.HasOne(o => o.Product)
                .WithMany()
                .HasForeignKey(o => o.ProductId);
        }
    }
}
