namespace Application.Domain.Inventories;

using System.Threading;
using System.Threading.Tasks;
using Application.Domain.__Common;
using Application.Domain.Inventories.Aggregate;
using Application.Domain.Inventories.ValueObjects;

public interface IInventoryRepository
{
    Task<IPagedList<Inventory>> FindAllPagedAsync(
        int pageIndex,
        int pageSize,
        CancellationToken ct = default
    );
    Task<Result<Inventory>> FindByIdAsync(InventoryId id, CancellationToken ct = default);
}
