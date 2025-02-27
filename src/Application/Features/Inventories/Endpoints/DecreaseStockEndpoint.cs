using Domain.Inventories;
using Domain.Inventories.Ids;
using Domain.Products.Ids;

namespace Application.Features.Inventories.Endpoints;

public static class DecreaseStockEndpoint
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
            Post("/inventory/decreaseStock");
            AllowAnonymous();
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var findResult = await _inventoryRepository.FindByIdAsync(req.Id, ct);

            if (findResult.IsNotFound())
                ThrowError("Inventory not found", StatusCodes.Status404NotFound);

            var inventory = findResult.Value!;

            var decreaseStock = inventory.DecreaseStock(req.Quantity, req.Reason);

            if (decreaseStock.IsInvalid())
            {
                await this.SendInvalidResponseAsync(decreaseStock, ct);
                return;
            }

            await SendAsync(
                new Response(decreaseStock.Value!.Id, decreaseStock.Value.ProductId, decreaseStock.Value.Quantity),
                StatusCodes.Status200OK,
                ct
            );
        }
    }
}
