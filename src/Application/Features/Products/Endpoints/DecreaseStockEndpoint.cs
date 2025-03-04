using Domain.Products;
using Domain.Products.Ids;
using JetBrains.Annotations;

namespace Application.Features.Products.Endpoints;

public sealed record DecreaseStockRequest(ProductId Id, Quantity Quantity, string Reason);

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public sealed record DecreaseStockResponse(ProductId Id, Quantity AvailableStock);

public sealed class DecreaseStockEndpoint : Endpoint<DecreaseStockRequest, DecreaseStockResponse>
{
    private readonly IProductRepository _productRepository;

    public DecreaseStockEndpoint(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public override void Configure()
    {
        Patch("/products/{id}/inventory/decreaseStock");
        Description(b =>
            b.Produces<DecreaseStockResponse>(StatusCodes.Status200OK, "application/json")
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound)
        );

        AllowAnonymous();
    }

    public override async Task HandleAsync(DecreaseStockRequest req, CancellationToken ct)
    {
        var product = await _productRepository.FindProductByIdAsync(req.Id, ct);
        if (product is null)
            await SendNotFoundAsync(ct);

        var decreaseStock = product!.DecreaseStock(req.Quantity, req.Reason);
        await this.SendErrorResponseIfResultFailedAsync(decreaseStock, ct);

        var productWithStockDecreased = decreaseStock.Object!;

        _productRepository.Update(productWithStockDecreased);
        await _productRepository.SaveChangesAsync(ct);

        await SendAsync(
            new DecreaseStockResponse(productWithStockDecreased.Id, productWithStockDecreased.GetAvailableStock()),
            StatusCodes.Status200OK,
            ct
        );
    }
}
