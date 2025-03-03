using Domain.Products;
using Domain.Products.Aggregate;
using Domain.Products.Ids;
using Domain.Products.ValueObjects;
using Infrastructure.Models;
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
            return Result.Failure();

        return ProductMapper.ToEntity(product);
    }

    public async Task<Result<Product>> FindProductByNameAsync(ProductName name, CancellationToken ct = default)
    {
        var product = await GetDefaultQuery().FirstOrDefaultAsync(product => product.Name == name, ct);

        if (product is null)
            return Result.Failure();

        return ProductMapper.ToEntity(product);
    }

    public async Task<Result<Product>> FindAnotherProductByNameAsync(
        ProductId id,
        ProductName name,
        CancellationToken ct = default
    )
    {
        var product = await GetDefaultQuery()
            .FirstOrDefaultAsync(product => product.Id != id && product.Name == name, ct);

        if (product is null)
            return Result.Failure();

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
