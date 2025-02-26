using Domain.Products;

namespace Application.Features.Products.UpdateProduct;

public sealed class UpdateProductEndpoint : Endpoint<Request, Response>
{
    private readonly IProductRepository _productRepository;

    public UpdateProductEndpoint(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public override void Configure()
    {
        Patch("/product/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var product = await _productRepository.FindProductByIdAsync(req.Id, ct);

        if (product.IsNotFound())
            ThrowError("Product was not found", StatusCodes.Status404NotFound);

        var anotherProductWithSameName = await _productRepository.FindProductByNameAsync(req.Name, ct);

        if (anotherProductWithSameName.WasFound())
        {
            ThrowError(
                $"There is already a product with the specified name ({req.Name})",
                StatusCodes.Status400BadRequest
            );
        }

        var updated = product.Value!.UpdateName(req.Name);
        if (updated.IsInvalid())
        {
            await this.SendInvalidResponseAsync(updated, ct);
            return;
        }

        var response = new Response(updated.Value!.Id, updated.Value.Name);

        await SendAsync(response, StatusCodes.Status200OK, ct);
    }
}
