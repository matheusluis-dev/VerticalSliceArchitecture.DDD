namespace Application.Common.Services;

using Application.Services;
using Microsoft.Extensions.Internal;

public static class ServicesConfiguration
{
    public static void AddCommonServices(this IServiceCollection services)
    {
        _ = services.AddSingleton<ISystemClock, DateTimeService>();
    }
}
