namespace Domain.Common.ValueObjects;

using Vogen;

[ValueObject<string>(conversions: Conversions.Default | Conversions.EfCoreValueConverter)]
public readonly partial struct Email;
