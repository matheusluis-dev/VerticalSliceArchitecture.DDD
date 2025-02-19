namespace Application.Features.Products.CreateProduct;

using Ardalis.Result;
using Domain.Products;
using Domain.Products.Entities;
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
        var productWithSameName = await _productRepository.FindProductByNameAsync(req.Name, ct);

        if (productWithSameName.WasFound())
            ThrowError($"Product with name '{req.Name}' already exists", StatusCodes.Status400BadRequest);

        var result = Product.Create(req.Name);
        if (result.Status is ResultStatus.Invalid)
        {
            await this.SendInvalidResponseAsync(result, ct);
            return;
        }

        var product = result.Value;

        await _productRepository.AddAsync(product, ct);
        await _context.SaveChangesAsync(ct);

        var response = new Response(product.Id, product.Name);

        await SendAsync(response, StatusCodes.Status201Created, ct);
    }
}
