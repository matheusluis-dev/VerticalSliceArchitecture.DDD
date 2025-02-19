namespace Infrastructure.Persistence.Inventories;

using Domain.Common;
using Domain.Inventories;
using Domain.Inventories.Aggregate;
using Domain.Inventories.ValueObjects;
using Infrastructure.Persistence.Tables;
using Microsoft.EntityFrameworkCore;

public sealed class InventoryRepository : IInventoryRepository
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<InventoryTable> _set;

    public InventoryRepository(ApplicationDbContext context)
    {
        _context = context;
        _set = _context.Set<InventoryTable>();
    }

    private IQueryable<InventoryTable> GetDefaultQuery()
    {
        return _set.AsQueryable().Include(o => o.Adjustments).Include(o => o.Reservations);
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
        var order = await _set.FindAsync([id], ct);

        if (order is null)
            return Result<Inventory>.NotFound();

        return InventoryMapper.ToEntity(order);
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
