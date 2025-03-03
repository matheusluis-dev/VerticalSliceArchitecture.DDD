using Domain.Products;
using Domain.Products.Aggregate;
using Domain.Products.Ids;
using Domain.Products.ValueObjects;

namespace Application.Features.Products;

public static class GetProductsEndpoint
{
    public sealed record Request : PagedRequest
    {
        public Request(int page, int size)
            : base(page, size) { }
    }

    public sealed record Response(ProductId ProductId, ProductName ProductName);

    public sealed record PagedResponse : PagedResponse<Response, Product>
    {
        public PagedResponse(IPagedList<Product> pagedList)
            : base(pagedList, product => new Response(product.Id, product.Name)) { }
    }

    public sealed class Endpoint : Endpoint<Request, PagedResponse>
    {
        private readonly IProductRepository _productRepository;

        public Endpoint(IProductRepository productRepository)
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
            var paged = await _productRepository.GetAllAsync(req.Page, req.Size, ct);
            var response = new PagedResponse(paged);

            await SendAsync(response, StatusCodes.Status200OK, ct);
        }
    }
}
