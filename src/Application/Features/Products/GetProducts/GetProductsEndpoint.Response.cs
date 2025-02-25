namespace Application.Features.Products.GetProducts;

using Application.BaseModels;
using Domain.Common;
using Domain.Products.Entities;
using Domain.Products.Ids;
using Domain.Products.ValueObjects;

public sealed record Response(ProductId ProductId, ProductName ProductName);

public sealed record PagedResponse : PagedResponse<Response, Product>
{
    public PagedResponse(IPagedList<Product> pagedList)
        : base(pagedList, product => new Response(product.Id, product.Name)) { }
}
