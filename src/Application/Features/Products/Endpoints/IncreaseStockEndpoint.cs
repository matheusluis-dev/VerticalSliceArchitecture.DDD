using Domain.Products;
using Domain.Products.Ids;
using JetBrains.Annotations;

namespace Application.Features.Products.Endpoints;

public sealed record IncreaseStockRequest(ProductId Id, Quantity Quantity, string Reason);

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public sealed record IncreaseStockResponse(ProductId Id, Quantity AvailableStock);

public sealed class IncreaseStockEndpoint : Endpoint<IncreaseStockRequest, IncreaseStockResponse>
{
    private readonly IProductRepository _productRepository;

    public IncreaseStockEndpoint(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public override void Configure()
    {
        Patch("/products/{id}/inventory/increaseStock");
        Description(b =>
            b.Produces<IncreaseStockResponse>(StatusCodes.Status200OK, "application/json")
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound)
        );

        AllowAnonymous();
    }

    public override async Task HandleAsync(IncreaseStockRequest req, CancellationToken ct)
    {
        var product = await _productRepository.FindProductByIdAsync(req.Id, ct);
        if (product is null)
            await SendNotFoundAsync(ct);

        var increaseStock = product!.IncreaseStock(req.Quantity, req.Reason);
        await this.SendErrorResponseIfResultFailedAsync(increaseStock, ct);

        var productWithStockIncreased = increaseStock.Object!;

        _productRepository.Update(productWithStockIncreased);
        await _productRepository.SaveChangesAsync(ct);

        await SendAsync(
            new IncreaseStockResponse(productWithStockIncreased.Id, productWithStockIncreased.GetAvailableStock()),
            StatusCodes.Status200OK,
            ct
        );
    }
}
