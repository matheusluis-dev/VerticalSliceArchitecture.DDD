using Domain.Products;

namespace Application.Features.Products.GetProducts;

public sealed class GetProductsEndpoint : Endpoint<Request, PagedResponse>
{
    private readonly IProductRepository _productRepository;

    public GetProductsEndpoint(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public override void Configure()
    {
        Get("/products/");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(req);

        var paged = await _productRepository.GetAllAsync(req.Page, req.Size, ct);
        var response = new PagedResponse(paged);

        await SendAsync(response, StatusCodes.Status200OK, ct);
    }
}
