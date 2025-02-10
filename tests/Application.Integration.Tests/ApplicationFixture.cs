namespace Application.Integration.Tests;

using FastEndpoints.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

public sealed class ApplicationFixture : AppFixture<Program>
{
    protected override void ConfigureApp(IWebHostBuilder a)
    {

    }

    protected override void ConfigureServices(IServiceCollection s)
    {
        // do test service registration here
    }
}
