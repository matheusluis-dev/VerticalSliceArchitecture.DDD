namespace Application.Domain.Orders.Models;

using Application.Domain.Common.ValueObjects;
using Application.Domain.Products.ValueObjects;

public sealed record AddOrderItemModel(ProductId ProductId, Quantity Quantity, Amount UnitPrice);
