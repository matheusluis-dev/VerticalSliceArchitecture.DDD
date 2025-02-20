namespace Application.Features.Products.DeleteProduct;

using System.Threading;
using System.Threading.Tasks;
using Domain.Inventories;
using Domain.Products;
using Domain.Products.Specifications;
using FastEndpoints;
using Microsoft.AspNetCore.Http;

public static partial class DeleteProduct
{
    public sealed class Endpoint : Endpoint<Request>
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductRepository _productRepository;
        private readonly IInventoryRepository _inventoryRepository;

        public Endpoint(
            IProductRepository productRepository,
            IInventoryRepository inventoryRepository,
            ApplicationDbContext context
        )
        {
            _productRepository = productRepository;
            _inventoryRepository = inventoryRepository;
            _context = context;
        }

        public override void Configure()
        {
            Delete("/product/{id}");
            AllowAnonymous();
        }

        public override async Task HandleAsync([NotNull] Request req, CancellationToken ct)
        {
            var findResult = await _productRepository.FindProductByIdAsync(req.Id, ct);

            if (findResult.IsNotFound())
            {
                await SendNotFoundAsync(ct);
                return;
            }

            var product = findResult.Value;

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
