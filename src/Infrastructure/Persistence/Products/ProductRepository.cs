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
    private readonly ProductMapper _mapper;

    private DbSet<ProductTable> ProductSet => _context.Set<ProductTable>();

    public ProductRepository(ApplicationDbContext context, ProductMapper productMapper)
    {
        _context = context;
        _mapper = productMapper;
    }

    public async Task<IPagedList<Product>> GetAllAsync(
        int page,
        int size,
        CancellationToken ct = default
    )
    {
        return await PagedList<Product>.CreateAsync(
            _mapper.ToEntityQueryable(ProductSet),
            page,
            size,
            ct
        );
    }

    public async Task<Result<Product>> FindProductByIdAsync(
        ProductId id,
        CancellationToken ct = default
    )
    {
        var product = await ProductSet.FindAsync([id], ct);

        if (product is null)
            return Result<Product>.NotFound();

        return _mapper.ToEntity(product);
    }

    public async Task<Result<Product>> FindProductByNameAsync(
        ProductName name,
        CancellationToken ct = default
    )
    {
        var product = await ProductSet.FirstOrDefaultAsync(product => product.Name == name, ct);

        if (product is null)
            return Result<Product>.NotFound();

        return _mapper.ToEntity(product);
    }

    public async Task AddAsync(Product product, CancellationToken ct = default)
    {
        await ProductSet.AddAsync(_mapper.ToTable(product), ct);
    }

    public void Delete(Product product)
    {
        ProductSet.Remove(_mapper.ToTable(product));
    }
}
