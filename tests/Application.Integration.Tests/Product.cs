using System.Net;
using Application.Features.Inventories.Endpoints.CreateInventory;
using Application.Features.Inventories.Endpoints.IncreaseStock;
using Application.Features.Products.CreateProduct;
using Application.Features.Products.DeleteProduct;
using Domain.Common.ValueObjects;
using Domain.Products.ValueObjects;
using FastEndpoints;
using Shouldly;

namespace Application.Integration.Tests;

public sealed class Product(ApplicationFixture app) : TestBase<ApplicationFixture>
{
    [Fact]
    public async Task Can_not_create_2_products_with_same_name()
    {
        // Arrange
        var requestWithSameName = new CreateProductEndpoint.Request(new ProductName(Guid.NewGuid().ToString()));

        // Act
        var (http1, _) = await app.ProductClient.POSTAsync<
            CreateProductEndpoint.Endpoint,
            CreateProductEndpoint.Request,
            CreateProductEndpoint.Response
        >(requestWithSameName);

        var (http2, _) = await app.ProductClient.POSTAsync<
            CreateProductEndpoint.Endpoint,
            CreateProductEndpoint.Request,
            CreateProductEndpoint.Response
        >(requestWithSameName);

        // Assert
        http1.StatusCode.ShouldBe(HttpStatusCode.Created);
        http2.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Should_not_delete_product_if_has_inventory_reservation_or_adjustment()
    {
        // Arrange
        var (_, product) = await app.ProductClient.POSTAsync<
            CreateProductEndpoint.Endpoint,
            CreateProductEndpoint.Request,
            CreateProductEndpoint.Response
        >(new CreateProductEndpoint.Request(new ProductName(Guid.NewGuid().ToString())));

        var (_, inventory) = await app.InventoryClient.POSTAsync<
            CreateInventoryEndpoint.Endpoint,
            CreateInventoryEndpoint.Request,
            CreateInventoryEndpoint.Response
        >(new CreateInventoryEndpoint.Request(product.Id, new Quantity(1)));

        await app.InventoryClient.POSTAsync<IncreaseStock.Endpoint, IncreaseStock.Request, IncreaseStock.Response>(
            new IncreaseStock.Request(inventory.Id, new Quantity(1), "Received from another company.")
        );

        // Act
        var delete = await app.ProductClient.DELETEAsync<DeleteProductEndpoint.Endpoint, DeleteProductEndpoint.Request>(
            new(product.Id)
        );

        // Assert
        delete.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
}
