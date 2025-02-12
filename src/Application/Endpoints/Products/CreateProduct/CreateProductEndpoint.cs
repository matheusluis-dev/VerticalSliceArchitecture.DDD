namespace Application.Endpoints.Products.CreateProduct;

using Application.Domain.Products.Entities;
using Application.Domain.Products.Repositories;
using Application.Infrastructure.Persistence;
using Ardalis.Result;
using FastEndpoints;
using Microsoft.AspNetCore.Http;

public sealed class CreateProductEndpoint : Endpoint<Request, Response>
{
    private readonly ApplicationDbContext _context;
    private readonly IProductRepository _productRepository;

    public CreateProductEndpoint(ApplicationDbContext context, IProductRepository productRepository)
    {
        _context = context;
        _productRepository = productRepository;
    }

    public override void Configure()
    {
        Post("/products");
        AllowAnonymous();
    }

    public override async Task HandleAsync([NotNull] Request req, CancellationToken ct)
    {
        var resultProductWithSameName = await _productRepository.FindProductByNameAsync(
            req.Name,
            ct
        );

        if (resultProductWithSameName.WasFound())
        {
            await SendAsync(null!, StatusCodes.Status400BadRequest, ct);
            return;
        }

        var resultProduct = Product.Create(req.Name);
        if (resultProduct.Status is ResultStatus.Invalid)
        {
            await this.SendInvalidResponseAsync(resultProduct, ct);
            return;
        }

        var product = resultProduct.Value;

        await _productRepository.CreateAsync(product, ct);
        await _context.SaveChangesAsync(ct);

        await SendAsync(new Response(product.Id, product.Name), StatusCodes.Status201Created, ct);
    }
}
