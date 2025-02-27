using Domain.Common.Entities;
using Domain.Common.ValueObjects;
using Domain.Inventories.Ids;
using Domain.Orders.Ids;
using Domain.Products.Ids;
using FastEndpoints;
using Infrastructure.Persistence.Converters.Ids;
using Infrastructure.Persistence.Converters.ValueObjects;
using Infrastructure.Persistence.Tables;
using JetBrains.Annotations;

namespace Infrastructure.Persistence;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public sealed class ApplicationDbContext : DbContext
{
    internal DbSet<OrderTable> Order { get; set; }
    internal DbSet<OrderItemTable> OrderItem { get; set; }
    internal DbSet<ProductTable> Product { get; set; }
    internal DbSet<InventoryTable> Inventory { get; set; }
    internal DbSet<AdjustmentTable> Adjustment { get; set; }
    internal DbSet<ReservationTable> Reservation { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        ChangeTracker.LazyLoadingEnabled = false;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseUpperSnakeCaseNamingConvention();
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        // Typed Ids
        configurationBuilder.Properties<AdjustmentId>().HaveConversion<AdjustmentIdConverter>();
        configurationBuilder.Properties<InventoryId>().HaveConversion<InventoryIdConverter>();
        configurationBuilder.Properties<OrderId>().HaveConversion<OrderIdConverter>();
        configurationBuilder.Properties<OrderItemId>().HaveConversion<OrderItemIdConverter>();
        configurationBuilder.Properties<ProductId>().HaveConversion<ProductIdConverter>();
        configurationBuilder.Properties<ReservationId>().HaveConversion<ReservationIdConverter>();

        // Value Objects
        configurationBuilder.Properties<Amount>().HaveConversion<AmountConverter>().HaveColumnType("decimal(14,4)");
        configurationBuilder.Properties<Email>().HaveConversion<EmailConverter>().HaveMaxLength(255);
        configurationBuilder.Properties<Quantity>().HaveConversion<QuantityConverter>().HaveMaxLength(10);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        NormalizeTableName(modelBuilder);
    }

    private static void NormalizeTableName(ModelBuilder modelBuilder)
    {
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            var entityName = entity.GetTableName();
            var withoutTableSuffix = entityName!.EndsWith("Table", StringComparison.Ordinal)
                ? entityName[0..^5]
                : entityName;

            entity.SetTableName(withoutTableSuffix);
        }
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await PublishDomainEventsAsync(cancellationToken);

        return await base.SaveChangesAsync(cancellationToken);
    }

    private async Task PublishDomainEventsAsync(CancellationToken cancellationToken = default)
    {
        var domainEvents = ChangeTracker
            .Entries<EntityBase>()
            .Select(entry => entry.Entity)
            .SelectMany(entity => entity.GetDomainEvents())
            .ToList();

        foreach (var domainEvent in domainEvents)
            await domainEvent.PublishAsync(Mode.WaitForAll, cancellationToken);
    }
}
