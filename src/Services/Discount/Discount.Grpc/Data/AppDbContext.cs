namespace Discount.Grpc.Data;

using Entities;
using Microsoft.EntityFrameworkCore;


public class AppDbContext : DbContext {

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<Coupon> Coupons { get; set; }

    override protected void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Coupon>()
            .HasData(
            new Coupon() { Id = 1, Amount = 10, ProductDescription = "description", ProductName = "S23" },
            new Coupon() { Id = 2, Amount = 20, ProductDescription = "description", ProductName = "16ProMax" });
    }

}
