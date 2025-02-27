using Domain.Inventories;
using Domain.Inventories.Ids;
using Domain.Products.Ids;

namespace Application.Features.Inventories.Endpoints;

public static class IncreaseStock
{
    public sealed record Request(InventoryId Id, Quantity Quantity, string Reason);

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
            var findResult = await _inventoryRepository.FindByIdAsync(req.Id, ct);

            if (findResult.IsNotFound())
                ThrowError("Inventory not found");

            var inventory = findResult.Value!;

            var increaseStock = inventory.IncreaseStock(req.Quantity, req.Reason);

            if (increaseStock.IsInvalid())
            {
                await this.SendInvalidResponseAsync(increaseStock, ct);
                return;
            }

            await SendAsync(
                new Response(increaseStock.Value!.Id, increaseStock.Value.ProductId, increaseStock.Value.Quantity),
                StatusCodes.Status200OK,
                ct
            );
        }
    }
}
