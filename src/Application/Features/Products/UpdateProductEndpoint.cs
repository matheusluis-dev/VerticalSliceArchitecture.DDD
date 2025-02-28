using Domain.Products;
using Domain.Products.Ids;
using Domain.Products.ValueObjects;
using JetBrains.Annotations;

namespace Application.Features.Products;

public static class UpdateProductEndpoint
{
    public sealed record Request(ProductId Id, ProductName Name);

    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public sealed record Response(ProductId Id, ProductName Name);

    public sealed class Endpoint : Endpoint<Request, Response>
    {
        private readonly IProductRepository _productRepository;

        public Endpoint(IProductRepository productRepository)
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
            var findProductResult = await _productRepository.FindProductByIdAsync(req.Id, ct);

            if (findProductResult.Failed)
                await SendNotFoundAsync(ct);

            var product = findProductResult.Value!;

            var findAnotherProductWithSameName = await _productRepository.FindAnotherProductByNameAsync(
                product.Id,
                req.Name,
                ct
            );

            if (findAnotherProductWithSameName.Succeed)
            {
                ThrowError(
                    $"There is already a product with the specified name ({req.Name})",
                    StatusCodes.Status400BadRequest
                );
            }

            var updateProduct = findProductResult.Value!.UpdateName(req.Name);
            await this.SendErrorResponseIfResultFailedAsync(updateProduct, ct);

            _productRepository.Update(updateProduct);
            await _productRepository.SaveChangesAsync(ct);

            var productUpdated = updateProduct.Value!;

            await SendAsync(new Response(productUpdated.Id, productUpdated.Name), StatusCodes.Status200OK, ct);
        }
    }
}
