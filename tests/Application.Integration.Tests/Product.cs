namespace Application.Integration.Tests;

using System.Net;
using Application.Features.Products.CreateProduct;
using Domain.Products.ValueObjects;
using FastEndpoints;
using Shouldly;

public sealed class Product(ApplicationFixture app) : TestBase<ApplicationFixture>
{
    [Fact]
    public async Task Can_not_create_2_products_with_same_name()
    {
        // Arrange
        var requestWithSameName = new Request(ProductName.From(Guid.NewGuid().ToString()));

        // Act
        var (http1, _) = await app.ProductClient.POSTAsync<CreateProductEndpoint, Request, Response>(
            requestWithSameName
        );

        var (http2, _) = await app.ProductClient.POSTAsync<CreateProductEndpoint, Request, Response>(
            requestWithSameName
        );

        // Assert
        http1.StatusCode.ShouldBe(HttpStatusCode.Created);
        http2.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
}
