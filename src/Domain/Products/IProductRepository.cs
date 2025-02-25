namespace Domain.Products;

using System.Threading.Tasks;
using Ardalis.Result;
using Domain.Common;
using Domain.Products.Entities;
using Domain.Products.Ids;
using Domain.Products.ValueObjects;

public interface IProductRepository
{
    Task<IPagedList<Product>> GetAllAsync(int page, int size, CancellationToken ct = default);
    Task<Result<Product>> FindProductByIdAsync(ProductId id, CancellationToken ct = default);
    Task<Result<Product>> FindProductByNameAsync(ProductName name, CancellationToken ct = default);
    Task AddAsync(Product product, CancellationToken ct = default);
    void Update(Product product);
    void Delete(Product product);
}
