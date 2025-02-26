using Domain.Products.Entities;
using Domain.Products.ValueObjects;

namespace Domain.Products;

public interface IProductRepository
{
    Task<IPagedList<Product>> GetAllAsync(int page, int size, CancellationToken ct = default);

    Task<Result<Product>> FindProductByIdAsync(ProductId id, CancellationToken ct = default);

    Task<Result<Product>> FindProductByNameAsync(ProductName name, CancellationToken ct = default);

    Task AddAsync(Product product, CancellationToken ct = default);

    void Update(Product product);

    void Delete(Product product);
}
