using Domain.Inventories;
using Domain.Inventories.Aggregate;
using Domain.Inventories.Ids;
using Infrastructure.Models;
using Infrastructure.Persistence.Tables;

namespace Infrastructure.Persistence.Repositories.Inventories;

public sealed class InventoryRepository : IInventoryRepository
{
    private readonly DbSet<InventoryTable> _set;

    public InventoryRepository(ApplicationDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        _set = context.Set<InventoryTable>();
    }

    private IQueryable<InventoryTable> GetDefaultQuery()
    {
        return _set.AsQueryable().Include(i => i.Product).Include(i => i.Adjustments).Include(i => i.Reservations);
    }

    public async Task AddAsync(Inventory product, CancellationToken ct = default)
    {
        await _set.AddAsync(InventoryMapper.ToTable(product), ct);
    }

    public void Update(Inventory product)
    {
        _set.Update(InventoryMapper.ToTable(product));
    }

    public async Task<Result<Inventory>> FindByIdAsync(InventoryId id, CancellationToken ct = default)
    {
        var inventory = await GetDefaultQuery().FirstOrDefaultAsync(i => i.Id == id, ct);

        if (inventory is null)
            return Result.NotFound();

        return InventoryMapper.ToEntity(inventory);
    }

    public async Task<IPagedList<Inventory>> FindAllPagedAsync(
        int pageIndex,
        int pageSize,
        CancellationToken ct = default
    )
    {
        return await PagedList<Inventory>.CreateAsync(
            InventoryMapper.ToEntityQueryable(GetDefaultQuery().AsNoTracking()),
            pageIndex,
            pageSize,
            ct
        );
    }
}
