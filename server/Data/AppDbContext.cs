using Microsoft.EntityFrameworkCore;
using PedidoApi.Models.Entities;

namespace PedidoApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<ProductEntity> Products => Set<ProductEntity>();
    public DbSet<OrderEntity> Orders => Set<OrderEntity>();
    public DbSet<OrderItemEntity> OrderItems => Set<OrderItemEntity>();
    public DbSet<BranchEntity> Branches => Set<BranchEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>(entity =>
        {
            entity.HasIndex(x => x.Email).IsUnique();
            entity.Property(x => x.Name).HasMaxLength(80);
            entity.Property(x => x.Email).HasMaxLength(160);
            entity.Property(x => x.Role).HasMaxLength(20);
            entity.Property(x => x.AuthProvider).HasMaxLength(20);
        });

        modelBuilder.Entity<ProductEntity>(entity =>
        {
            entity.Property(x => x.Name).HasMaxLength(80);
            entity.Property(x => x.Description).HasMaxLength(250);
            entity.Property(x => x.ImageUrl).HasMaxLength(300);
            entity.Property(x => x.Price).HasPrecision(10, 2);
        });

        modelBuilder.Entity<OrderEntity>(entity =>
        {
            entity.Property(x => x.BranchName).HasMaxLength(100);
            entity.Property(x => x.BranchAddress).HasMaxLength(250);
            entity.Property(x => x.CustomerName).HasMaxLength(80);
            entity.Property(x => x.CustomerEmail).HasMaxLength(160);
            entity.Property(x => x.Status).HasMaxLength(40);
            entity.Property(x => x.DeliveryType).HasMaxLength(60);
            entity.Property(x => x.PaymentMethod).HasMaxLength(60);
            entity.Property(x => x.Total).HasPrecision(10, 2);
            entity.HasMany(x => x.Items)
                .WithOne(x => x.Order)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<BranchEntity>(entity =>
        {
            entity.HasIndex(x => x.Username).IsUnique();
            entity.Property(x => x.Name).HasMaxLength(100);
            entity.Property(x => x.Address).HasMaxLength(250);
            entity.Property(x => x.Username).HasMaxLength(80);
        });

        modelBuilder.Entity<OrderItemEntity>(entity =>
        {
            entity.Property(x => x.ProductName).HasMaxLength(80);
            entity.Property(x => x.UnitPrice).HasPrecision(10, 2);
            entity.Property(x => x.Subtotal).HasPrecision(10, 2);
        });
    }
}
