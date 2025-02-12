namespace Application.Infrastructure.Persistence.Inventories;

using Application.Domain.Common;
using Application.Domain.Inventories;
using Application.Domain.Inventories.Aggregate;
using Application.Domain.Inventories.ValueObjects;
using Application.Infrastructure.Persistence.Tables;
using Microsoft.EntityFrameworkCore;

public sealed class InventoryRepository : IInventoryRepository
{
    private readonly DbSet<InventoryTable> _set;
    private readonly InventoryMapper _mapper;

    public InventoryRepository(ApplicationDbContext context, InventoryMapper inventoryMapper)
    {
        ArgumentNullException.ThrowIfNull(context);

        _set = context.Set<InventoryTable>();
        _mapper = inventoryMapper;
    }

    private IQueryable<InventoryTable> GetDefaultQuery()
    {
        return _set.AsQueryable().Include(o => o.Adjustments).Include(o => o.Reservations);
    }

    public async Task<Inventory?> FindByIdAsync(InventoryId id, CancellationToken ct = default)
    {
        var order = await _set.FindAsync([id], ct);

        if (order is null)
            return null;

        return _mapper.ToEntity(order);
    }

    public async Task<IPagedList<Inventory>> FindAllPagedAsync(
        int pageIndex,
        int pageSize,
        CancellationToken ct = default
    )
    {
        return await PagedList<Inventory>.CreateAsync(
            _mapper.ToEntityQueryable(GetDefaultQuery().AsNoTracking()),
            pageIndex,
            pageSize,
            ct
        );
    }
}
