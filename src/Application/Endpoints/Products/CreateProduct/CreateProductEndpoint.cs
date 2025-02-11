namespace Application.Endpoints.Products.CreateProduct;

using Application.Domain.Products.Repositories;
using Application.Infrastructure.Persistence;
using FastEndpoints;
using Microsoft.AspNetCore.Http;

public sealed class CreateProductEndpoint : Endpoint<Request, Response, OrderMapper>
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
        Validator<CreateOrderValidator>();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var product = Map.ToEntity(req);

        if (await _productRepository.FindProductByNameAsync(product.Name, ct) is not null)
        {
            await SendAsync(null!, StatusCodes.Status400BadRequest, ct);
            return;
        }

        await _productRepository.CreateAsync(product, ct);
        await _context.SaveChangesAsync(ct);

        await SendAsync(Map.FromEntity(product), StatusCodes.Status201Created, ct);
    }
}
