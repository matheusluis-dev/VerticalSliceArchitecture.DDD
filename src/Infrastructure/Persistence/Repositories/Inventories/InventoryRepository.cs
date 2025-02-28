using Domain.Inventories;
using Domain.Inventories.Aggregate;
using Domain.Inventories.Ids;
using Infrastructure.Persistence.Tables;

namespace Infrastructure.Persistence.Repositories.Inventories;

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
            return Result.Failure();

        return InventoryMapper.ToEntity(inventory);
    }

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return _context.SaveChangesAsync(ct);
    }
}
