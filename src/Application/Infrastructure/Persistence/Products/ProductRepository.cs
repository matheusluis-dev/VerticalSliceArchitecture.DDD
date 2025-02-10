namespace Application.Infrastructure.Persistence.Products;

using Application.Domain.Products.Entities;
using Application.Domain.Products.Repositories;
using Application.Domain.Products.ValueObjects;
using Application.Infrastructure.Persistence.Products.Mappers;
using Application.Infrastructure.Persistence.Products.Tables;
using Microsoft.EntityFrameworkCore;

public sealed class ProductRepository : IProductRepository
{
    private readonly DbSet<ProductTable> _productSet;

    public ProductRepository(ApplicationDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        _productSet = context.Product;
    }

    public async Task<Product?> FindProductByIdAsync(ProductId id, CancellationToken ct = default)
    {
        var order = await _productSet.FindAsync([id], ct);

        if (order is null)
            return null;

        return order.ToEntity();
    }

    public async Task<Product?> CreateAsync(Product product, CancellationToken ct = default)
    {
        await _productSet.AddAsync(product.FromEntity(), ct);

        return product;
    }
}
