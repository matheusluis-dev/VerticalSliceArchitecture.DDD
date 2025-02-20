namespace Application.Features.Inventories.Endpoints.DecreaseStock;

using System.Threading;
using System.Threading.Tasks;
using Domain.Inventories;
using Microsoft.AspNetCore.Http;

public sealed class DecreaseStockEndpoint : Endpoint<Request, Response>
{
    private readonly IInventoryRepository _inventoryRepository;

    public DecreaseStockEndpoint(IInventoryRepository inventoryRepository)
    {
        _inventoryRepository = inventoryRepository;
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
            ThrowError("Inventory not found", StatusCodes.Status404NotFound);

        var inventory = findResult.Value;

        var result = inventory.DecreaseStock(req.Quantity, req.Reason);

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
