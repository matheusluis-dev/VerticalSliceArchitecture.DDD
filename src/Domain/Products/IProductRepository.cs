using Domain.Products.Aggregate;
using Domain.Products.ValueObjects;

namespace Domain.Products;

public interface IProductRepository
{
    Task<Product?> FindProductByIdAsync(ProductId id, CancellationToken ct = default);

    Task<Product?> FindProductByNameAsync(ProductName name, CancellationToken ct = default);

    Task<Product?> FindAnotherProductByNameAsync(ProductId id, ProductName name, CancellationToken ct = default);

    Task AddAsync(Product product, CancellationToken ct = default);

    void Update(Product product);

    Task DeleteAsync(IEnumerable<ProductId> ids, CancellationToken ct = default);

    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
