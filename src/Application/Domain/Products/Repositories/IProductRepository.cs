namespace Application.Domain.Products.Repositories;

using System.Threading.Tasks;
using Application.Domain.Products.Entities;
using Application.Domain.Products.ValueObjects;
using Ardalis.Result;

public interface IProductRepository
{
    Task<Result<Product>> FindProductByIdAsync(ProductId id, CancellationToken ct = default);
    Task<Result<Product>> FindProductByNameAsync(ProductName name, CancellationToken ct = default);
    Task CreateAsync(Product product, CancellationToken ct = default);
}
