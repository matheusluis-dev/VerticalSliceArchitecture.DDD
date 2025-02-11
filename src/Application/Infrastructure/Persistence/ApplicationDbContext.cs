namespace Application.Infrastructure.Persistence;

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Application.Domain.Common.Entities;
using Application.Domain.User.ValueObjects;
using Application.Infrastructure.Persistence.Inventories.Tables;
using Application.Infrastructure.Persistence.Orders.Tables;
using Application.Infrastructure.Persistence.Products.Tables;
using Application.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

public sealed class ApplicationDbContext : DbContext
{
    private readonly IDateTimeService _dateTime;

    public DbSet<OrderTable> Order { get; set; }
    public DbSet<OrderItemTable> OrderItem { get; set; }
    public DbSet<ProductTable> Product { get; set; }
    public DbSet<InventoryTable> Inventory { get; set; }

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IDateTimeService dateTime
    )
        : base(options)
    {
        _dateTime = dateTime;

        ChangeTracker.LazyLoadingEnabled = false;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseUpperSnakeCaseNamingConvention();
    }

    protected override void ConfigureConventions(
        [NotNull] ModelConfigurationBuilder configurationBuilder
    )
    {
        configurationBuilder.ApplyVogenEfConvertersFromAssembly(
            typeof(ApplicationDbContext).Assembly
        );
    }

    protected override void OnModelCreating([NotNull] ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<OrderTable>(model => model.HasKey(m => m.Id))
            .Entity<OrderItemTable>(model =>
            {
                model.HasKey(m => m.Id);
                model.HasIndex(m => m.Id);

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

                model.HasIndex(m => m.Name).IsUnique(true);
            })
            .Entity<InventoryTable>(model =>
            {
                model.HasKey(m => m.Id);
                model.HasIndex(m => m.Id);

                model
                    .HasOne(m => m.Product)
                    .WithOne(m => m.Inventory)
                    .HasForeignKey<InventoryTable>(i => i.ProductId)
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

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var now = _dateTime.UtcNow;

        foreach (var changedEntity in ChangeTracker.Entries())
        {
            if (changedEntity.Entity is not IAuditable auditableEntity)
                continue;

            if (changedEntity.State is EntityState.Added)
            {
                auditableEntity.Created = now.DateTime;
                auditableEntity.LastModified = null;
                auditableEntity.CreatedBy = UserId.Create();
                auditableEntity.LastModifiedBy = UserId.Create();
            }

            if (changedEntity.State is EntityState.Modified)
            {
                auditableEntity.Created = now.DateTime;
                auditableEntity.LastModifiedBy = UserId.Create();
                break;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
