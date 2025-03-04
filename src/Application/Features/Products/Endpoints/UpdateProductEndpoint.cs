using Domain.Products;
using Domain.Products.Ids;
using Domain.Products.ValueObjects;
using JetBrains.Annotations;

namespace Application.Features.Products.Endpoints;

public sealed record UpdateProductRequest(ProductId Id, ProductName Name);

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public sealed record UpdateProductResponse(ProductId Id, ProductName Name);

public sealed class UpdateProductEndpoint : Endpoint<UpdateProductRequest, UpdateProductResponse>
{
    private readonly IProductRepository _productRepository;

    public UpdateProductEndpoint(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public override void Configure()
    {
        Patch("/products/{id}");
        Description(b =>
            b.Produces<UpdateProductResponse>(StatusCodes.Status200OK, "application/json")
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound)
        );

        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateProductRequest req, CancellationToken ct)
    {
        var product = await _productRepository.FindProductByIdAsync(req.Id, ct);
        if (product is null)
            await SendNotFoundAsync(ct);

        var findAnotherProductWithSameName = await _productRepository.FindAnotherProductByNameAsync(
            product!.Id,
            req.Name,
            ct
        );
        if (findAnotherProductWithSameName is not null)
        {
            ThrowError(
                $"There is already a product with the specified name ({req.Name})",
                StatusCodes.Status400BadRequest
            );
        }

        var updateProduct = product.UpdateName(req.Name);
        await this.SendErrorResponseIfResultFailedAsync(updateProduct, ct);

        _productRepository.Update(updateProduct);
        await _productRepository.SaveChangesAsync(ct);

        var productUpdated = updateProduct.Object!;

        await SendAsync(new UpdateProductResponse(productUpdated.Id, productUpdated.Name), StatusCodes.Status200OK, ct);
    }
}
