namespace Application;

using System.Text.Json.Serialization;
using Domain.Inventories;
using Domain.Inventories.Services;
using Domain.Orders;
using Domain.Products;
using FastEndpoints;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Inventories;
using Infrastructure.Persistence.Orders;
using Infrastructure.Persistence.Products;
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
        services.AddSingleton<IEmailService, EmailService>();

        AddOrder();
        AddInventory();
        AddProduct();

        return services;

        void AddOrder()
        {
            services.AddSingleton<OrderMapper>();
            services.AddSingleton<OrderItemMapper>();

            services.AddScoped<IOrderRepository, OrderRepository>();
        }

        void AddInventory()
        {
            services.AddSingleton<InventoryMapper>();
            services.AddSingleton<AdjustmentMapper>();
            services.AddSingleton<ReservationMapper>();

            services.AddSingleton<AdjustInventoryStockService>();

            services.AddScoped<IInventoryRepository, InventoryRepository>();
        }

        void AddProduct()
        {
            services.AddSingleton<ProductMapper>();

            services.AddScoped<IProductRepository, ProductRepository>();
        }
    }
}
