namespace Application.Endpoints.Inventories.IncreaseStock;

using Application.Domain.Common.ValueObjects;
using Application.Domain.Inventories.ValueObjects;

public sealed record Request(InventoryId Id, Quantity Quantity, string Reason);
