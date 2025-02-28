using Domain.Products;
using Domain.Products.Entities;
using Domain.Products.Ids;
using Domain.Products.ValueObjects;
using JetBrains.Annotations;

namespace Application.Features.Products;

public static class CreateProductEndpoint
{
    public sealed record Request(ProductName Name);

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
            Post("/products");
            AllowAnonymous();
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var findProductWithSameName = await _productRepository.FindProductByNameAsync(req.Name, ct);

            if (findProductWithSameName.Succeed)
                ThrowError($"Product with name '{req.Name}' already exists", StatusCodes.Status400BadRequest);

            var createProduct = Product.Create(req.Name);
            await this.SendErrorResponseIfResultFailedAsync(createProduct, ct);

            var productCreated = createProduct.Value!;

            await _productRepository.AddAsync(productCreated, ct);
            await _productRepository.SaveChangesAsync(ct);

            var response = new Response(productCreated.Id, productCreated.Name);

            await SendAsync(response, StatusCodes.Status201Created, ct);
        }
    }
}
