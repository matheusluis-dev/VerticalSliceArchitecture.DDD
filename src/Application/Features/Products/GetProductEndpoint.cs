using Domain.Products;
using Domain.Products.Ids;
using Domain.Products.ValueObjects;

namespace Application.Features.Products;

public static class GetProductEndpoint
{
    public record Request(ProductId Id);

    public record Response(ProductId ProductId, ProductName ProductName);

    public sealed class Endpoint : Endpoint<Request, Response>
    {
        private readonly IProductRepository _repository;

        public Endpoint(IProductRepository repository)
        {
            _repository = repository;
        }

        public override void Configure()
        {
            Get("/product/{id}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            ArgumentNullException.ThrowIfNull(req);

            var findProductResult = await _repository.FindProductByIdAsync(req.Id, ct);

            if (findProductResult.Failed)
                await SendNotFoundAsync(ct);

            var product = findProductResult.Value!;
            var response = new Response(product.Id, product.Name);

            await SendAsync(response, StatusCodes.Status200OK, ct);
        }
    }
}
