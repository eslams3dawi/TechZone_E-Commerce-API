using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechZone.DAL.Models;

namespace TechZone.DAL.Database
{
    public class TechZoneContext : IdentityDbContext<ApplicationUser>
    {
        public TechZoneContext(DbContextOptions<TechZoneContext> options): base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TechZoneContext).Assembly);

            //Soft delete
            modelBuilder.Entity<Product>().HasQueryFilter(p => !p.IsDeleted);
            modelBuilder.Entity<ShoppingCart>().HasQueryFilter(sc => !sc.Product.IsDeleted);
            modelBuilder.Entity<OrderDetail>().HasQueryFilter(od => !od.Product.IsDeleted);

            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public virtual DbSet<OrderHeader> OrdersHeaders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
    }
}
