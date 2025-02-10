namespace Application.Endpoints.Products.CreateProduct;

using System.ComponentModel.DataAnnotations;
using Application.Domain.Products.ValueObjects;

public sealed record Response([Required] ProductId Id, ProductName Name);
