namespace Application.Features.Products.GetProducts;

public sealed record Request : PagedRequest
{
    public Request(int page, int size)
        : base(page, size) { }
}
