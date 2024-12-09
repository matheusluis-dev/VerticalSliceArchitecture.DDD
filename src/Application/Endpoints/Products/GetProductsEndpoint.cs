namespace Application.Endpoints.Products;

using System.Threading;
using System.Threading.Tasks;

public sealed record Request(int Id, string Name);

public sealed record Response(int Id, string Name);

#pragma warning disable S2094 // Classes should not be empty
public sealed class LolEndpoint;
#pragma warning restore S2094 // Classes should not be empty

public sealed class GetProductsEndpoint : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Get("/products");
        AllowAnonymous();
    }

    public override Task HandleAsync(Request req, CancellationToken ct)
    {
        return Task.CompletedTask;
    }
}
