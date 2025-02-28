using Domain.Inventories;
using Domain.Inventories.Ids;
using Domain.Inventories.Services;
using Domain.Products;
using Domain.Products.Ids;
using JetBrains.Annotations;

namespace Application.Features.Inventories.Endpoints;

public static class CreateInventoryEndpoint
{
    public sealed record Request(ProductId ProductId, Quantity Quantity);

    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public sealed record Response(InventoryId Id, ProductId ProductId, Quantity Quantity);

    public sealed class Endpoint : Endpoint<Request, Response>
    {
        private readonly IProductRepository _productRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly CreateInventoryService _createInventory;

        public Endpoint(
            IProductRepository productRepository,
            IInventoryRepository inventoryRepository,
            CreateInventoryService createInventoryService
        )
        {
            _productRepository = productRepository;
            _inventoryRepository = inventoryRepository;
            _createInventory = createInventoryService;
        }

        public override void Configure()
        {
            Post("/inventory");
            AllowAnonymous();
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var findProductResult = await _productRepository.FindProductByIdAsync(req.ProductId, ct);

            if (findProductResult.Failed)
                await SendNotFoundAsync(ct);

            var inventory = _createInventory.CreateForProduct(findProductResult, req.Quantity);

            await this.SendErrorResponseIfResultFailedAsync(inventory, ct);

            await _inventoryRepository.AddAsync(inventory, ct);
            await _inventoryRepository.SaveChangesAsync(ct);

            await SendAsync(
                new Response(inventory.Value!.Id, inventory.Value.ProductId, inventory.Value.Quantity),
                StatusCodes.Status201Created,
                ct
            );
        }
    }
}
