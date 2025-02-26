using Domain.Inventories.Aggregate;

namespace Domain.Inventories;

public interface IInventoryRepository
{
    Task<IPagedList<Inventory>> FindAllPagedAsync(int pageIndex, int pageSize, CancellationToken ct = default);

    Task<Result<Inventory>> FindByIdAsync(InventoryId id, CancellationToken ct = default);

    Task AddAsync(Inventory product, CancellationToken ct = default);

    void Update(Inventory product);
}
