namespace Application;

using System.Text.Json.Serialization;
using Application.Domain.Inventories;
using Application.Domain.Orders;
using Application.Domain.Products.Repositories;
using Application.Infrastructure.Persistence;
using Application.Infrastructure.Persistence.Inventories;
using Application.Infrastructure.Persistence.Orders;
using Application.Infrastructure.Persistence.Products;
using Application.Infrastructure.Services;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddConfiguredFastEndpoints(this IServiceCollection services)
    {
        services.AddFastEndpoints(options =>
        {
            options.SourceGeneratorDiscoveredTypes = DiscoveredTypes.All;
            options.IncludeAbstractValidators = true;
        });

        services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter())
        );

        services.Configure<Microsoft.AspNetCore.Mvc.JsonOptions>(options =>
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())
        );

        return services;
    }

    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services;
    }

    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("VSA")
            );
        }
        else
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
                )
            );
        }

        services.AddTransient<IDateTimeService, DateTimeService>();

        AddMappers();
        AddRepositories();

        return services;

        void AddMappers()
        {
            services.AddSingleton<OrderMapper>();
            services.AddSingleton<OrderItemMapper>();

            services.AddSingleton<InventoryMapper>();
            services.AddSingleton<AdjustmentMapper>();
            services.AddSingleton<ReservationMapper>();

            services.AddSingleton<ProductMapper>();
        }

        void AddRepositories()
        {
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IInventoryRepository, InventoryRepository>();
        }
    }
}
