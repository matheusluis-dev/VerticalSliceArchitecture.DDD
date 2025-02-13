namespace Application.Endpoints.Products.GetProducts;

using Application.Domain.__Common;
using Application.Domain.Products.Entities;
using Application.Domain.Products.ValueObjects;
using Application.Endpoints.__Common;

public sealed record Response(ProductId ProductId, ProductName ProductName);

public sealed record PagedResponse : PagedResponse<Response, Product>
{
    public PagedResponse(IPagedList<Product> pagedList)
        : base(pagedList, product => new Response(product.Id, product.Name)) { }
}
