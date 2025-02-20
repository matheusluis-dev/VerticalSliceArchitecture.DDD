namespace Application.Integration.Tests;

using System.Net;
using Application.Features.Inventories.Endpoints.CreateInventory;
using Application.Features.Inventories.Endpoints.IncreaseStock;
using Application.Features.Products.CreateProduct;
using Application.Features.Products.DeleteProduct;
using Domain.Common.ValueObjects;
using Domain.Products.ValueObjects;
using FastEndpoints;
using Shouldly;

public sealed class Product(ApplicationFixture app) : TestBase<ApplicationFixture>
{
    [Fact]
    public async Task Can_not_create_2_products_with_same_name()
    {
        // Arrange
        var requestWithSameName = new CreateProduct.Request(ProductName.From(Guid.NewGuid().ToString()));

        // Act
        var (http1, _) = await app.ProductClient.POSTAsync<
            CreateProduct.Endpoint,
            CreateProduct.Request,
            CreateProduct.Response
        >(requestWithSameName);

        var (http2, _) = await app.ProductClient.POSTAsync<
            CreateProduct.Endpoint,
            CreateProduct.Request,
            CreateProduct.Response
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
            CreateProduct.Endpoint,
            CreateProduct.Request,
            CreateProduct.Response
        >(new CreateProduct.Request(ProductName.From(Guid.NewGuid().ToString())));

        var (_, inventory) = await app.InventoryClient.POSTAsync<
            CreateInventory.Endpoint,
            CreateInventory.Request,
            CreateInventory.Response
        >(new CreateInventory.Request(product.Id, Quantity.From(1)));

        await app.InventoryClient.POSTAsync<IncreaseStock.Endpoint, IncreaseStock.Request, IncreaseStock.Response>(
            new IncreaseStock.Request(inventory.Id, Quantity.From(1), "Received from another company.")
        );

        // Act
        var delete = await app.ProductClient.DELETEAsync<DeleteProduct.Endpoint, DeleteProduct.Request>(
            new(product.Id)
        );

        // Assert
        delete.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
}
