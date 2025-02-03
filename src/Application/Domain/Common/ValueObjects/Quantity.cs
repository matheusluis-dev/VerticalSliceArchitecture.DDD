namespace Application.Domain.Common.ValueObjects;

using Vogen;

[ValueObject<int>(conversions: Conversions.Default | Conversions.EfCoreValueConverter)]
public readonly partial struct Quantity;
