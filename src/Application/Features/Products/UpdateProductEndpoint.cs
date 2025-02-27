using Domain.Products;
using Domain.Products.Ids;
using Domain.Products.ValueObjects;

namespace Application.Features.Products;

public static class UpdateProductEndpoint
{
    public sealed record Request(ProductId Id, ProductName Name);

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
}
