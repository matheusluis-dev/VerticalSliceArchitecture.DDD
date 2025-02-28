using Domain.Inventories.Aggregate;

namespace Domain.Inventories;

public interface IInventoryRepository
{
    Task<Result<Inventory>> FindByIdAsync(InventoryId id, CancellationToken ct = default);

    Task AddAsync(Inventory product, CancellationToken ct = default);

    void Update(Inventory product);

    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
