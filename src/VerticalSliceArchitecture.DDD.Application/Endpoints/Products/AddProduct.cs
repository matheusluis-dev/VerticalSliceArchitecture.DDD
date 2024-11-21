namespace VerticalSliceArchitecture.DDD.Application.Endpoints.Products;

public sealed record Request(string? Name, decimal? Price);
public sealed record Response(string? Name, decimal? Price);

public sealed class AddProduct : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Post("/api/addProduct");
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        await Task.FromResult(0);
    }
}
