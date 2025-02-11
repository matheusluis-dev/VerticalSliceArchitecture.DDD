namespace Application.Domain.Products.Repositories;

using System.Threading.Tasks;
using Application.Domain.Products.Entities;
using Application.Domain.Products.ValueObjects;

public interface IProductRepository
{
    Task<Product?> FindProductByIdAsync(ProductId id, CancellationToken ct = default);
    Task<Product?> FindProductByNameAsync(ProductName name, CancellationToken ct = default);
    Task<Product?> CreateAsync(Product product, CancellationToken ct = default);
}
