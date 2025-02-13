namespace Application.Endpoints.Products.GetProducts;

using Application.Endpoints.__Common;

public sealed record Request : PagedRequest
{
    public Request(int page, int size)
        : base(page, size) { }
}
