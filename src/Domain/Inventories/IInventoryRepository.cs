namespace Domain.Inventories;

using System.Threading;
using System.Threading.Tasks;
using Domain.Common;
using Domain.Inventories.Aggregate;
using Domain.Inventories.ValueObjects;

public interface IInventoryRepository
{
    Task<IPagedList<Inventory>> FindAllPagedAsync(int pageIndex, int pageSize, CancellationToken ct = default);

    Task<Result<Inventory>> FindByIdAsync(InventoryId id, CancellationToken ct = default);

    Task AddAsync(Inventory product, CancellationToken ct = default);

    void Update(Inventory product);
}
