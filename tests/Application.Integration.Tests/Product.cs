namespace Application.Integration.Tests;

using System.Net;
using Application.Features.Products.CreateProduct;
using Domain.Products.ValueObjects;
using FastEndpoints;
using FastEndpoints.Testing;
using Shouldly;

public sealed class Product(ApplicationFixture app) : TestBase<ApplicationFixture>
{
    [Fact]
    public async Task Create_product()
    {
        // Arrange
        var productName = "Product";
        var request = new Request(ProductName.From(productName));

        // Act
        var (response, dto) = await app.ProductClient.POSTAsync<
            CreateProductEndpoint,
            Request,
            Response
        >(request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        dto.Id.Value.ShouldNotBe(Guid.Empty);
        dto.Name.Value.ShouldBe(productName.ToUpperInvariant());
    }

    [Fact]
    public async Task Should_not_allow_two_products_with_same_name_on_product_creation()
    {
        // Arrange
        var productName = "Product";
        var request = new Request(ProductName.From(productName));

        // Act
        _ = await app.ProductClient.POSTAsync<CreateProductEndpoint, Request, Response>(request);
        var (response, _) = await app.ProductClient.POSTAsync<
            CreateProductEndpoint,
            Request,
            Response
        >(request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
}
