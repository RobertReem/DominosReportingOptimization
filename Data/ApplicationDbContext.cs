using Microsoft.EntityFrameworkCore;
using DominosReportingOptimization.Models;

namespace DominosReportingOptimization.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Store> Stores { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //configure relationships
            modelBuilder.Entity<Order>()
                .HasOne<Store>()
                .WithMany()
                .HasForeignKey(o => o.StoreId);

            modelBuilder.Entity<OrderItem>()
                .HasOne<Order>()
                .WithMany()
                .HasForeignKey(oi => oi.OrderId);

            modelBuilder.Entity<OrderItem>()
                .HasOne<Product>()
                .WithMany()
                .HasForeignKey(oi => oi.ProductId);
        }

    }
}