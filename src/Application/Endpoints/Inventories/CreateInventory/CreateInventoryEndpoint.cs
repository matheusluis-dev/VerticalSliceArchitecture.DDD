namespace Application.Endpoints.Inventories.CreateInventory;

using System.Threading;
using System.Threading.Tasks;
using Application.Domain.Inventories;
using Application.Domain.Inventories.Aggregate;
using Application.Domain.Products.Repositories;
using Application.Domain.Products.Specifications;
using Microsoft.AspNetCore.Http;

public sealed class CreateInventoryEndpoint : Endpoint<Request, Response>
{
    private readonly IProductRepository _productRepository;
    private readonly IInventoryRepository _inventoryRepository;

    public CreateInventoryEndpoint(
        IProductRepository productRepository,
        IInventoryRepository inventoryRepository
    )
    {
        _productRepository = productRepository;
        _inventoryRepository = inventoryRepository;
    }

    public override void Configure()
    {
        Post("/inventory");
        AllowAnonymous();
    }

    public override async Task HandleAsync([NotNull] Request req, CancellationToken ct)
    {
        var product = await _productRepository.FindProductByIdAsync(req.ProductId, ct);

        if (product.IsNotFound())
            ThrowError("Product not found", StatusCodes.Status404NotFound);

        if (new HasInventorySpecification().IsSatisfiedBy(product))
        {
            ThrowError(
                $"Product already has an Inventory, with ID '{product.Value.Id}'",
                StatusCodes.Status400BadRequest
            );
        }

        var inventory = Inventory.CreateForProduct(product.Value.Id, req.Quantity);

        if (inventory.IsInvalid())
        {
            await this.SendInvalidResponseAsync(inventory, ct);
            return;
        }

        await _inventoryRepository.AddAsync(inventory, ct);
        await _inventoryRepository.SaveChangesAsync(ct);

        await SendAsync(
            new Response(inventory.Value.Id, inventory.Value.ProductId, inventory.Value.Quantity),
            StatusCodes.Status201Created,
            ct
        );
    }
}
