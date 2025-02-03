namespace Application;

using System.Text.Json.Serialization;
using Application.Domain.Orders.Repositories;
using Application.Domain.Orders.Specifications.Builder;
using Application.Infrastructure.Orders;
using Application.Infrastructure.Persistence;
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
            options.SourceGeneratorDiscoveredTypes = DiscoveredTypes.All
        );

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

        AddRepositories();
        AddSpecifications();

        return services;

        void AddRepositories()
        {
            services.AddScoped<IOrderRepository, OrderRepository>();
        }

        void AddSpecifications()
        {
            services.AddScoped<OrderSpecificationBuilder, OrderSpecificationBuilder>();
        }
    }
}
