namespace Application.Infrastructure.Persistence.Products;

using Application.Domain.Products.Entities;
using Application.Domain.Products.Repositories;
using Application.Domain.Products.ValueObjects;
using Application.Infrastructure.Persistence.Tables;
using Ardalis.Result;
using Microsoft.EntityFrameworkCore;

public sealed class ProductRepository : IProductRepository
{
    private readonly DbSet<ProductTable> _set;
    private readonly ProductMapper _mapper;

    public ProductRepository(ApplicationDbContext context, ProductMapper productMapper)
    {
        ArgumentNullException.ThrowIfNull(context);

        _set = context.Product;
        _mapper = productMapper;
    }

    public async Task<Result<Product>> FindProductByIdAsync(
        ProductId id,
        CancellationToken ct = default
    )
    {
        var product = await _set.FindAsync([id], ct);

        if (product is null)
            return Result<Product>.NotFound();

        return _mapper.ToEntity(product);
    }

    public async Task<Result<Product>> FindProductByNameAsync(
        ProductName name,
        CancellationToken ct = default
    )
    {
        var product = await _set.FirstOrDefaultAsync(product => product.Name == name, ct);

        if (product is null)
            return Result<Product>.NotFound();

        return _mapper.ToEntity(product);
    }

    public async Task CreateAsync(Product product, CancellationToken ct = default)
    {
        await _set.AddAsync(_mapper.ToTable(product), ct);
    }
}
