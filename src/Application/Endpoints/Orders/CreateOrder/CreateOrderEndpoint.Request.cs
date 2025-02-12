namespace Application.Endpoints.Orders.CreateOrder;

using Application.Domain.Common.ValueObjects;
using Application.Domain.Products.ValueObjects;

public sealed record Request(Email CustomerEmail, IEnumerable<RequestItems> Items);

public sealed record RequestItems(ProductId ProductId, Quantity Quantity, Amount UnitPrice);
