namespace Application.Features.Products.GetProducts;

using Application.BaseModels;

public sealed record Request : PagedRequest
{
    public Request(int page, int size)
        : base(page, size) { }
}
