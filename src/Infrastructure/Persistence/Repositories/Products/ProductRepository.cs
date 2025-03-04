using Domain.Products;
using Domain.Products.Aggregate;
using Domain.Products.Ids;
using Domain.Products.ValueObjects;
using Infrastructure.Persistence.Tables;

namespace Infrastructure.Persistence.Repositories.Products;

public sealed class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<ProductTable> _set;

    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
        _set = _context.Set<ProductTable>();
    }

#nullable  disable

    private IQueryable<ProductTable> GetDefaultQuery()
    {
        return _set.AsQueryable()
            .Include(p => p.Inventory)
            .ThenInclude(i => i.Adjustments)
            .Include(p => p.Inventory)
            .ThenInclude(i => i.Reservations);
    }

#nullable restore

    public async Task<Product?> FindProductByIdAsync(ProductId id, CancellationToken ct = default)
    {
        var product = await GetDefaultQuery()!.FirstOrDefaultAsync(p => p.Id == id, ct);

        if (product is null)
            return null;

        return ProductMapper.ToEntity(product);
    }

    public async Task<Product?> FindProductByNameAsync(ProductName name, CancellationToken ct = default)
    {
        var product = await GetDefaultQuery().FirstOrDefaultAsync(product => product.Name == name, ct);

        if (product is null)
            return null;

        return ProductMapper.ToEntity(product);
    }

    public async Task<Product?> FindAnotherProductByNameAsync(
        ProductId id,
        ProductName name,
        CancellationToken ct = default
    )
    {
        var product = await GetDefaultQuery()
            .FirstOrDefaultAsync(product => product.Id != id && product.Name == name, ct);

        if (product is null)
            return null;

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

    public Task DeleteAsync(IEnumerable<ProductId> ids, CancellationToken ct = default)
    {
        return _set.Where(product => ids.Contains(product.Id)).ExecuteDeleteAsync(ct);
    }

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return _context.SaveChangesAsync(ct);
    }
}
