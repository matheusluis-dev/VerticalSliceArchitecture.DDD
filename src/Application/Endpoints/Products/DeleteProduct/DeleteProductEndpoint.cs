namespace Application.Endpoints.Products.DeleteProduct;

using System.Threading;
using System.Threading.Tasks;
using Application.Domain.Inventories;
using Application.Domain.Inventories.Specifications;
using Application.Domain.Products.Repositories;
using FastEndpoints;
using Microsoft.AspNetCore.Http;

public sealed class DeleteProductEndpoint : Endpoint<Request>
{
    private readonly IProductRepository _productRepository;
    private readonly IInventoryRepository _inventoryRepository;

    public DeleteProductEndpoint(
        IProductRepository productRepository,
        IInventoryRepository inventoryRepository
    )
    {
        _productRepository = productRepository;
        _inventoryRepository = inventoryRepository;
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

        var resultInventory = await _inventoryRepository.FindByIdAsync(
            product.InventoryId.GetValueOrDefault(),
            ct
        );

        if (
            resultInventory.WasFound()
            && !new InventoryWasNeverAdjustedSpecification().IsSatisfiedBy(resultInventory)
        )
        {
            await SendAsync(
                "Can not delete product with a inventory that already was modified",
                StatusCodes.Status400BadRequest,
                ct
            );

            return;
        }

        _productRepository.Delete(product);
        await _productRepository.SaveChangesAsync(ct);

        await SendOkAsync(ct);
    }
}
