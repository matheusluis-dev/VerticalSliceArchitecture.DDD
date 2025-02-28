using Domain.Products;
using Domain.Products.Ids;
using Domain.Products.Specifications;

namespace Application.Features.Products;

public static class DeleteProductEndpoint
{
    public sealed record Request(ProductId Id);

    public sealed class Endpoint : Endpoint<Request>
    {
        private readonly IProductRepository _productRepository;

        public Endpoint(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public override void Configure()
        {
            Delete("/product/{id}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var findProductResult = await _productRepository.FindProductByIdAsync(req.Id, ct);

            if (findProductResult.Failed)
                await SendNotFoundAsync(ct);

            var product = findProductResult.Value!;
            if (product.HasInventory && !new ProductCanBeDeletedSpecification().IsSatisfiedBy(product))
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
}
