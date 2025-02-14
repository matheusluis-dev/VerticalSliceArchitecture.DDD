namespace Application.Features.Products.DeleteProduct;

using System.Threading;
using System.Threading.Tasks;
using Domain.Inventories;
using Domain.Inventories.Specifications;
using Domain.Products;
using FastEndpoints;
using Microsoft.AspNetCore.Http;

public sealed class DeleteProductEndpoint : Endpoint<Request>
{
    private readonly ApplicationDbContext _context;
    private readonly IProductRepository _productRepository;
    private readonly IInventoryRepository _inventoryRepository;

    public DeleteProductEndpoint(
        IProductRepository productRepository,
        IInventoryRepository inventoryRepository,
        ApplicationDbContext context
    )
    {
        _productRepository = productRepository;
        _inventoryRepository = inventoryRepository;
        _context = context;
    }

    public override void Configure()
    {
        Delete("/product/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync([NotNull] Request req, CancellationToken ct)
    {
        var findResult = await _productRepository.FindProductByIdAsync(req.Id, ct);

        if (findResult.IsNotFound())
        {
            await SendNotFoundAsync(ct);
            return;
        }

        var product = findResult.Value;

        if (product.Inventory is not null)
        {
            var resultInventory = await _inventoryRepository.FindByIdAsync(
                product.Inventory.Id,
                ct
            );

            if (
                resultInventory.WasFound()
                && !new InventoryWasNeverAdjustedSpecification().IsSatisfiedBy(resultInventory)
            )
            {
                ThrowError(
                    "Can not delete product with a inventory that already was modified",
                    StatusCodes.Status400BadRequest
                );

                return;
            }
        }

        _productRepository.Delete(product);
        await _context.SaveChangesAsync(ct);

        await SendOkAsync(ct);
    }
}
