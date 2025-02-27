using Domain.Products;
using Domain.Products.Entities;
using Domain.Products.Ids;
using Domain.Products.ValueObjects;

namespace Application.Features.Products;

public static class CreateProductEndpoint
{
    public sealed record Request(ProductName Name);

    public sealed record Response(ProductId Id, ProductName Name);

    public sealed class Endpoint : Endpoint<Request, Response>
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductRepository _productRepository;

        public Endpoint(ApplicationDbContext context, IProductRepository productRepository)
        {
            _context = context;
            _productRepository = productRepository;
        }

        public override void Configure()
        {
            Post("/products");
            AllowAnonymous();
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var productWithSameName = await _productRepository.FindProductByNameAsync(req.Name, ct);

            if (productWithSameName.WasFound())
                ThrowError($"Product with name '{req.Name}' already exists", StatusCodes.Status400BadRequest);

            var result = Product.Create(req.Name);
            if (result.Status is ResultStatus.Invalid)
            {
                await this.SendInvalidResponseAsync(result, ct);
                return;
            }

            var product = result.Value!;

            await _productRepository.AddAsync(product, ct);
            await _context.SaveChangesAsync(ct);

            var response = new Response(product.Id, product.Name);

            await SendAsync(response, StatusCodes.Status201Created, ct);
        }
    }
}
