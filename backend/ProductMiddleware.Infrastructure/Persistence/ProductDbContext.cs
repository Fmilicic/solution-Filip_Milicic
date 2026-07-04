using Microsoft.EntityFrameworkCore;
using ProductMiddleware.Infrastructure.Persistence.Entities;

namespace ProductMiddleware.Infrastructure.Persistence;

public sealed class ProductDbContext : DbContext
{
    public ProductDbContext(DbContextOptions<ProductDbContext> options)
        : base(options)
    {
    }

    public DbSet<ProductEntity> Products => Set<ProductEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductEntity>(entity =>
        {
            entity.HasKey(product => product.Id);
            entity.Property(product => product.Name).IsRequired().HasMaxLength(200);
            entity.Property(product => product.Description).HasMaxLength(4000);
            entity.Property(product => product.ImageUrl).HasMaxLength(1000);
            entity.Property(product => product.Category).HasMaxLength(100);
            entity.Property(product => product.Price).HasColumnType("TEXT");
        });
    }
}
