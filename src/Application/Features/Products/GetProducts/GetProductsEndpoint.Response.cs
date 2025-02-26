using Domain.Products.Entities;
using Domain.Products.Ids;
using Domain.Products.ValueObjects;

namespace Application.Features.Products.GetProducts;

public sealed record Response(ProductId ProductId, ProductName ProductName);

public sealed record PagedResponse : PagedResponse<Response, Product>
{
    public PagedResponse(IPagedList<Product> pagedList)
        : base(pagedList, product => new Response(product.Id, product.Name)) { }
}
