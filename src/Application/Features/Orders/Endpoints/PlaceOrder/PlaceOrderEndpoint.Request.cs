namespace Application.Features.Orders.Endpoints.PlaceOrder;

using Domain.Common.ValueObjects;
using Domain.Products.ValueObjects;

public sealed record Request(Email CustomerEmail, IEnumerable<RequestItems> Items);

public sealed record RequestItems(ProductId ProductId, Quantity Quantity, Amount UnitPrice);
