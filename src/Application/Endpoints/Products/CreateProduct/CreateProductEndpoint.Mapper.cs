namespace Application.Endpoints.Products.CreateProduct;

using Application.Domain.Products.Entities;
using Application.Domain.Products.ValueObjects;
using FastEndpoints;

public sealed class OrderMapper : Mapper<Request, Response, Product>
{
    public override Product ToEntity(Request r)
    {
        ArgumentNullException.ThrowIfNull(r);

        return new() { Id = ProductId.Create(), Name = r.ProductName.GetValueOrDefault() };
    }

    public override Response FromEntity(Product e)
    {
        ArgumentNullException.ThrowIfNull(e);

        return new(e.Id, e.Name);
    }
}
