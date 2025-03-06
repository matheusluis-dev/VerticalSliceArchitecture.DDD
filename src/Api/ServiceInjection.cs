using System.Text.Json.Serialization;
using Application;
using Application.Converters;
using Application.Features.Products.Services;
using Domain.Common.Contracts;
using Domain.Orders;
using Domain.Orders.Services;
using Domain.Products;
using Domain.Products.Services;
using FastEndpoints;
using FastEndpoints.Swagger;
using Infrastructure.Job;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories.Orders;
using Infrastructure.Persistence.Repositories.Products;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Api;

public static class ServiceInjection
{
    public static IServiceCollection AddConfiguredFastEndpoints(this IServiceCollection services)
    {
        services.AddFastEndpoints(options => options.SourceGeneratorDiscoveredTypes = DiscoveredTypes.All!);
        services.SwaggerDocument();

        services.AddJobQueues<JobRecord, JobStorage>();

        services.AddSingleton(typeof(IRequestBinder<>), typeof(TypedIdRequestBinder<>));

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
            {
                services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("VSA"));
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
            services.AddSingleton<IDateTimeService, DateTimeService>();
            services.AddScoped<IEmailService, EmailService>();
        }

        void AddOrder()
        {
            services.AddScoped<OrderPlacementService>();
            services.AddScoped<OrderItemManagementService>();

            services.AddScoped<IOrderRepository, OrderRepository>();
        }

        void AddInventory()
        {
            services.AddScoped<CreateAdjustmentService>();
            services.AddScoped<StockReleaseService>();
            services.AddScoped<StockReservationService>();
        }

        void AddProduct()
        {
            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddScoped<GetProductForOrderItemsService>();
        }
    }
}
