namespace VerticalSliceArchitecture.DDD.Application.Endpoints.Products;

internal sealed record Request(string? Name, decimal? Price);

internal sealed record Response(string? Name, decimal? Price);

internal sealed class AddProduct : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Post("/api/addProduct");
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        _ = await Task.FromResult(0);
    }
}
