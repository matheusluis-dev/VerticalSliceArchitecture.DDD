using Domain.Products;
using Domain.Products.Ids;
using Domain.Products.Specifications;

namespace Application.Features.Products;

public static class DeleteProductEndpoint
{
    public sealed record Request(ProductId Id);

    public sealed class Endpoint : Endpoint<Request>
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductRepository _productRepository;

        public Endpoint(IProductRepository productRepository, ApplicationDbContext context)
        {
            _productRepository = productRepository;
            _context = context;
        }

        public override void Configure()
        {
            Delete("/product/{id}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var findResult = await _productRepository.FindProductByIdAsync(req.Id, ct);

            if (findResult.IsNotFound())
            {
                await SendNotFoundAsync(ct);
                return;
            }

            var product = findResult.Value!;

            if (product.HasInventory && !new ProductCanBeDeletedSpecification().IsSatisfiedBy(product))
            {
                ThrowError(
                    "Can not delete product with a inventory that already was modified",
                    StatusCodes.Status400BadRequest
                );

                return;
            }

            _productRepository.Delete(product);
            await _context.SaveChangesAsync(ct);

            await SendOkAsync(ct);
        }
    }
}
