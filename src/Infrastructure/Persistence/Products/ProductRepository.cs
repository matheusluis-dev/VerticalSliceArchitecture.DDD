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
    private readonly DbSet<ProductTable> _set;

    public ProductRepository(ApplicationDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        _set = context.Set<ProductTable>();
    }

    private IQueryable<ProductTable> GetDefaultQuery()
    {
        return _set.AsQueryable().Include(o => o.Inventory);
    }

    public async Task<IPagedList<Product>> GetAllAsync(int page, int size, CancellationToken ct = default)
    {
        return await PagedList<Product>.CreateAsync(ProductMapper.ToEntityQueryable(GetDefaultQuery()), page, size, ct);
    }

    public async Task<Result<Product>> FindProductByIdAsync(ProductId id, CancellationToken ct = default)
    {
        var product = await GetDefaultQuery().FirstOrDefaultAsync(p => p.Id == id, ct);

        if (product is null)
            return Result<Product>.NotFound();

        return ProductMapper.ToEntity(product);
    }

    public async Task<Result<Product>> FindProductByNameAsync(ProductName name, CancellationToken ct = default)
    {
        var product = await GetDefaultQuery().FirstOrDefaultAsync(product => product.Name == name, ct);

        if (product is null)
            return Result<Product>.NotFound();

        return ProductMapper.ToEntity(product);
    }

    public async Task AddAsync(Product product, CancellationToken ct = default)
    {
        await _set.AddAsync(ProductMapper.ToTable(product), ct);
    }

    public void Update(Product product)
    {
        _set.Update(ProductMapper.ToTable(product));
    }

    public void Delete(Product product)
    {
        _set.Remove(ProductMapper.ToTable(product));
    }
}
