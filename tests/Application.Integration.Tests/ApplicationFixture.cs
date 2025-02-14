namespace Application.Integration.Tests;

using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using FastEndpoints.Testing;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public sealed class ApplicationFixture : AppFixture<Program>
{
    public HttpClient ProductClient { get; private set; } = null!;

    protected override async ValueTask SetupAsync()
    {
        // Apply migrations to the test database
        ApplyMigrations();

        ProductClient = CreateClient();
    }

    protected override ValueTask TearDownAsync()
    {
        ProductClient.Dispose();

        return ValueTask.CompletedTask;
    }

    protected override void ConfigureServices([NotNull] IServiceCollection s)
    {
        var descriptor = s.Single(d =>
            d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>)
        );

        s.Remove(descriptor);

        s.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                "Server=127.0.0.1,1433;Database=VSA_TEST;User ID=sa;Password=<YourStrong@Passw0rd>;TrustServerCertificate=True;"
            )
        );
    }

    private void ApplyMigrations()
    {
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        try
        {
            // Ensure the database is created and migrations are applied
            dbContext.Database.EnsureDeleted(); // Optional: Ensure a clean slate
            dbContext.Database.EnsureCreated();
            dbContext.Database.Migrate();
        }
        catch { }
    }
}
