namespace Application.Integration.Tests;

using Application.Domain.Products.ValueObjects;
using Application.Endpoints.Products.CreateProduct;
using FastEndpoints;
using FastEndpoints.Testing;

public sealed class Product(ApplicationFixture app) : TestBase<ApplicationFixture>
{
    [Fact]
    public async Task Test1()
    {
        var (rsp, res) = await app.CreateClient()
            .POSTAsync<CreateProductEndpoint, Request, Response>(
                new Request(ProductName.From("Product 1"))
            );

        Console.WriteLine();
    }
}
