namespace Application.Infrastructure.Persistence;

using System.Diagnostics.CodeAnalysis;
using Application.Domain.Inventories.ValueObjects;
using Application.Domain.Orders.ValueObjects;
using Application.Domain.Products.ValueObjects;
using Application.Infrastructure.Persistence.Inventories.Tables;
using Application.Infrastructure.Persistence.Orders.Tables;
using Application.Infrastructure.Persistence.Products.Tables;
using Microsoft.EntityFrameworkCore;

public sealed class ApplicationDbContext : DbContext
{
    public DbSet<OrderTable> Order { get; set; }
    public DbSet<OrderItemTable> OrderItem { get; set; }
    public DbSet<ProductTable> Product { get; set; }
    public DbSet<InventoryTable> Inventory { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        ChangeTracker.LazyLoadingEnabled = false;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseUpperSnakeCaseNamingConvention();
    }

    protected override void OnModelCreating([NotNull] ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<OrderTable>(model =>
            {
                model.HasKey(m => m.Id);
                model.HasIndex(m => m.Id);

                model.Property(m => m.Id).HasVogenConversion();
            })
            .Entity<OrderItemTable>(model =>
            {
                model.HasKey(m => m.Id);
                model.HasIndex(m => m.Id);

                model.Property(m => m.Id).HasVogenConversion();

                model
                    .HasOne(m => m.Order)
                    .WithMany(m => m.OrderItems)
                    .HasForeignKey(m => m.OrderId)
                    .OnDelete(DeleteBehavior.Restrict);

                model
                    .HasOne(m => m.Product)
                    .WithMany(m => m.OrderItems)
                    .HasForeignKey(m => m.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);
            })
            .Entity<ProductTable>(model =>
            {
                model.HasKey(m => m.Id);
                model.HasIndex(m => m.Id);

                model.Property(m => m.Id).HasVogenConversion();
                model.Property(m => m.Name).HasVogenConversion();
            })
            .Entity<InventoryTable>(model =>
            {
                model.HasKey(m => m.Id);
                model.HasIndex(m => m.Id);

                model.Property(m => m.Id).HasVogenConversion();

                model
                    .HasOne(m => m.Product)
                    .WithOne(m => m.Inventory)
                    .OnDelete(DeleteBehavior.Restrict);
            });

        NormalizeTableName(modelBuilder);
    }

    private static void NormalizeTableName(ModelBuilder modelBuilder)
    {
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            var entityName = entity.GetTableName();
            var withoutModel = entityName!.EndsWith("Table", StringComparison.Ordinal)
                ? entityName[0..^5]
                : entityName;

            entity.SetTableName(withoutModel);
        }
    }
}
