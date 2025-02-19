namespace Infrastructure.Persistence.Products;

using Ardalis.Result;
using Domain.Products;
using Domain.Products.Entities;
using Domain.Products.ValueObjects;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Tables;
using Microsoft.EntityFrameworkCore;

public sealed class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;

    private DbSet<ProductTable> ProductSet => _context.Set<ProductTable>();

    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IPagedList<Product>> GetAllAsync(int page, int size, CancellationToken ct = default)
    {
        return await PagedList<Product>.CreateAsync(ProductMapper.ToEntityQueryable(ProductSet), page, size, ct);
    }

    public async Task<Result<Product>> FindProductByIdAsync(ProductId id, CancellationToken ct = default)
    {
        var product = await ProductSet.FindAsync([id], ct);

        if (product is null)
            return Result<Product>.NotFound();

        return ProductMapper.ToEntity(product);
    }

    public async Task<Result<Product>> FindProductByNameAsync(ProductName name, CancellationToken ct = default)
    {
        var product = await ProductSet.FirstOrDefaultAsync(product => product.Name == name, ct);

        if (product is null)
            return Result<Product>.NotFound();

        return ProductMapper.ToEntity(product);
    }

    public async Task AddAsync(Product product, CancellationToken ct = default)
    {
        await ProductSet.AddAsync(ProductMapper.ToTable(product), ct);
    }

    public void Delete(Product product)
    {
        ProductSet.Remove(ProductMapper.ToTable(product));
    }
}
