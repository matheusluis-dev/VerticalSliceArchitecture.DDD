namespace Domain.Orders.Models;

using Domain.Common.ValueObjects;
using Domain.Products.Entities;

public sealed record AddOrderItemModel(Product Product, Quantity Quantity, Amount UnitPrice);
