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
    public class PaymentConfigurations : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("Payments");
            builder.HasKey(p => p.PaymentId);

            builder.Property(p => p.PaymentDate);

            builder.Property(p => p.Amount)
                .HasPrecision(8, 2)
                .IsRequired();

            builder.Property(p => p.Method)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(p => p.Status)
                .IsRequired();

            builder.Property(p => p.PaymentIntentId);
            builder.Property(p => p.SessionId);

            builder.HasOne(p => p.Order)
                .WithOne(p => p.Payment)
                .HasForeignKey<Payment>(o => o.OrderHeaderId);
        }
    }
}
