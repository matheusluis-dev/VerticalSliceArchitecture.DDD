namespace Application.Configuration;

using Application.Services;
using Microsoft.Extensions.Internal;

public static class DependencyInjectionConfiguration
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton<ISystemClock, DateTimeService>();
    }
}
