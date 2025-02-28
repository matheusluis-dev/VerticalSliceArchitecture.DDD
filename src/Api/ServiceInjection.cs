using System.Text.Json.Serialization;
using Domain.Common.Contracts;
using Domain.Inventories;
using Domain.Inventories.Services;
using Domain.Orders;
using Domain.Orders.Services;
using Domain.Products;
using FastEndpoints;
using FastEndpoints.Swagger;
using Infrastructure.Job;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories.Inventories;
using Infrastructure.Persistence.Repositories.Orders;
using Infrastructure.Persistence.Repositories.Products;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Application;

public static class ServiceInjection
{
    public static IServiceCollection AddConfiguredFastEndpoints(this IServiceCollection services)
    {
        services.AddFastEndpoints(options => options.SourceGeneratorDiscoveredTypes = DiscoveredTypes.All!);

        services.SwaggerDocument();

        services.AddJobQueues<JobRecord, JobStorage>();

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

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        AddDatabase();
        AddJobStorage();

        AddInfrastructureServices();

        AddOrder();
        AddInventory();
        AddProduct();

        return services;

        void AddDatabase()
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
                services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("VSA"));
            else
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(
                        configuration.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
                    )
                );
            }
        }

        void AddJobStorage()
        {
            services.Configure<MongoDbSettings>(configuration.GetSection("MongoDbSettings"));

            services.AddSingleton<IMongoClient>(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
                return new MongoClient(settings.ConnectionString);
            });

            services.AddSingleton(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
                var client = sp.GetRequiredService<IMongoClient>();
                return client.GetDatabase(settings.DatabaseName)!;
            });
        }

        void AddInfrastructureServices()
        {
            services.AddTransient<IDateTimeService, DateTimeService>();
            services.AddSingleton<IEmailService, EmailService>();
        }

        void AddOrder()
        {
            services.AddSingleton<OrderPlacementService>();
            services.AddSingleton<OrderItemManagementService>();

            services.AddScoped<IOrderRepository, OrderRepository>();
        }

        void AddInventory()
        {
            services.AddSingleton<CreateAdjustmentService>();
            services.AddSingleton<StockReleaseService>();
            services.AddSingleton<StockReservationService>();
            services.AddSingleton<CreateInventoryService>();

            services.AddScoped<IInventoryRepository, InventoryRepository>();
        }

        void AddProduct()
        {
            services.AddScoped<IProductRepository, ProductRepository>();
        }
    }
}
