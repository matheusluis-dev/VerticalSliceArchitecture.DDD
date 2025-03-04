using Domain.Products;
using Domain.Products.Ids;

namespace Application.Features.Products.Endpoints;

public sealed record DeleteProductRequest(ProductId Id);

public sealed class DeleteProductEndpoint : Endpoint<DeleteProductRequest>
{
    private readonly IProductRepository _productRepository;

    public DeleteProductEndpoint(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public override void Configure()
    {
        Delete("/products/{id}");
        Description(b =>
            b.Produces(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound)
        );

        AllowAnonymous();
    }

    public override async Task HandleAsync(DeleteProductRequest req, CancellationToken ct)
    {
        var product = await _productRepository.FindProductByIdAsync(req.Id, ct);
        if (product is null)
            await SendNotFoundAsync(ct);

        if (!product!.CanBeDeleted)
        {
            ThrowError(
                "Can not delete product with a inventory that already was modified",
                StatusCodes.Status400BadRequest
            );
        }

        await _productRepository.DeleteAsync([product.Id], ct);

        await SendOkAsync(ct);
    }
}
