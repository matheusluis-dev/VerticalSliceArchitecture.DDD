using Domain.Inventories;
using Domain.Inventories.Services;
using Domain.Products;

namespace Application.Features.Inventories.Endpoints.CreateInventory;

public static partial class CreateInventoryEndpoint
{
    internal sealed class Endpoint : Endpoint<Request, Response>
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductRepository _productRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly CreateInventoryService _createInventory;

        public Endpoint(
            ApplicationDbContext context,
            IProductRepository productRepository,
            IInventoryRepository inventoryRepository,
            CreateInventoryService createInventoryService
        )
        {
            _context = context;
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
            var product = await _productRepository.FindProductByIdAsync(req.ProductId, ct);

            if (product.IsNotFound())
                ThrowError("Product not found", StatusCodes.Status404NotFound);

            var inventory = _createInventory.CreateForProduct(product, req.Quantity);

            if (inventory.IsInvalid())
            {
                await this.SendInvalidResponseAsync(inventory, ct);
                return;
            }

            await _inventoryRepository.AddAsync(inventory, ct);
            await _context.SaveChangesAsync(ct);

            await SendAsync(
                new Response(inventory.Value!.Id, inventory.Value.ProductId, inventory.Value.Quantity),
                StatusCodes.Status201Created,
                ct
            );
        }
    }
}
