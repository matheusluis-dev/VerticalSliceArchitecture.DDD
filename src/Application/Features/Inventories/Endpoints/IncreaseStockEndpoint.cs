using Domain.Inventories;
using Domain.Products.Ids;
using JetBrains.Annotations;

namespace Application.Features.Inventories.Endpoints;

public static class IncreaseStock
{
    public sealed record Request(InventoryId Id, Quantity Quantity, string Reason);

    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public sealed record Response(InventoryId Id, ProductId ProductId, Quantity Quantity);

    public sealed class Endpoint : Endpoint<Request, Response>
    {
        private readonly IInventoryRepository _inventoryRepository;

        public Endpoint(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        public override void Configure()
        {
            Post("/inventory/increaseStock");
            AllowAnonymous();
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var findInventoryResult = await _inventoryRepository.FindByIdAsync(req.Id, ct);

            if (findInventoryResult.Failed)
                await SendNotFoundAsync(ct);

            var inventory = findInventoryResult.Value!;

            var increaseStock = inventory.IncreaseStock(req.Quantity, req.Reason);
            await this.SendErrorResponseIfResultFailedAsync(increaseStock, ct);

            var inventoryWithStockIncreased = increaseStock.Value!;

            _inventoryRepository.Update(inventoryWithStockIncreased);
            await _inventoryRepository.SaveChangesAsync(ct);

            await SendAsync(
                new Response(
                    inventoryWithStockIncreased.Id,
                    inventoryWithStockIncreased.ProductId,
                    inventoryWithStockIncreased.Quantity
                ),
                StatusCodes.Status200OK,
                ct
            );
        }
    }
}
