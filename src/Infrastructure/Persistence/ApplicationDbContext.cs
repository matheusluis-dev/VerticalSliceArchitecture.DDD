namespace Infrastructure.Persistence;

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Domain.Common.Entities;
using FastEndpoints;
using Infrastructure.Persistence.Tables;
using Microsoft.EntityFrameworkCore;

public sealed class ApplicationDbContext : DbContext
{
    public DbSet<JobRecord> JobRecord { get; set; }

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
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        modelBuilder.Entity<JobRecord>().HasKey(j => j.TrackingID);

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
        await PublishDomainEventsAsync();

        return await base.SaveChangesAsync(cancellationToken);
    }

    private async Task PublishDomainEventsAsync()
    {
        var domainEvents = ChangeTracker
            .Entries<EntityBase>()
            .Select(entry => entry.Entity)
            .SelectMany(entity => entity.GetDomainEvents())
            .ToList();

        foreach (var domainEvent in domainEvents)
            await domainEvent.PublishAsync(Mode.WaitForAll);
    }
}
