namespace Application.Features.Inventories.Endpoints.DecreaseStock;

using System.Threading;
using System.Threading.Tasks;
using Domain.Inventories;
using Domain.Inventories.Services;
using Microsoft.AspNetCore.Http;

public sealed class DecreaseStockEndpoint : Endpoint<Request, Response>
{
    private readonly IInventoryRepository _inventoryRepository;
    private readonly AdjustInventoryStockService _adjustInventoryStock;

    public DecreaseStockEndpoint(
        IInventoryRepository inventoryRepository,
        AdjustInventoryStockService adjustInventoryStock
    )
    {
        _inventoryRepository = inventoryRepository;
        _adjustInventoryStock = adjustInventoryStock;
    }

    public override void Configure()
    {
        Post("/inventory/decreaseStock");
        AllowAnonymous();
    }

    public override async Task HandleAsync([NotNull] Request req, CancellationToken ct)
    {
        var findResult = await _inventoryRepository.FindByIdAsync(req.Id, ct);

        if (findResult.IsNotFound())
            ThrowError("Inventory not found");

        var inventory = findResult.Value;

        var result = _adjustInventoryStock.DecreaseQuantity(inventory, req.Quantity, req.Reason);

        if (result.IsInvalid())
        {
            await this.SendInvalidResponseAsync(result, ct);
            return;
        }

        await SendAsync(
            new Response(result.Value.Id, result.Value.ProductId, result.Value.Quantity),
            StatusCodes.Status200OK,
            ct
        );
    }
}
