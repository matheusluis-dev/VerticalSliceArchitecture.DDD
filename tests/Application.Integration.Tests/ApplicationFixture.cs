namespace Application.Integration.Tests;

using Infrastructure.Persistence;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public sealed class ApplicationFixture : AppFixture<Program>
{
    private MongoDbContainer _mongoContainer = null!;
    private MsSqlContainer _mssqlContainer = null!;

    const string MONGO_DATABASE = "TestingDB";
    const string MONGO_USERNAME = "root";
    const string MONGO_PASSWORD = "password";

    public HttpClient ProductClient { get; private set; }
    public HttpClient InventoryClient { get; private set; }

    protected override async ValueTask PreSetupAsync()
    {
        _mongoContainer = new MongoDbBuilder()
            .WithImage("mongo")
            .WithUsername(MONGO_USERNAME)
            .WithPassword(MONGO_PASSWORD)
            .WithCommand("mongod")
            .Build();

        _mssqlContainer = new MsSqlBuilder().Build();

        await _mssqlContainer.StartAsync(Cancellation);
        await _mongoContainer.StartAsync(Cancellation);
    }

    protected override ValueTask SetupAsync()
    {
        ProductClient = CreateClient();
        InventoryClient = CreateClient();

        return ValueTask.CompletedTask;
    }

    protected override void ConfigureApp(IWebHostBuilder a)
    {
        a.ConfigureAppConfiguration(c =>
        {
            c.AddInMemoryCollection(
                new Dictionary<string, string?>(StringComparer.Ordinal)
                {
                    { "Mongo:Host", _mongoContainer.Hostname },
                    { "Mongo:Port", Convert.ToString(_mongoContainer.GetMappedPublicPort(27017)) },
                    { "Mongo:DbName", MONGO_DATABASE },
                    { "Mongo:UserName", MONGO_USERNAME },
                    { "Mongo:Password", MONGO_PASSWORD },
                }
            );
        });

        a.ConfigureTestServices(services =>
        {
            var descriptor = services.SingleOrDefault(s =>
                s.ServiceType == typeof(DbContextOptions<ApplicationDbContext>)
            );

            if (descriptor is not null)
                services.Remove(descriptor);

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(
                    _mssqlContainer.GetConnectionString(),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
                );
            });
        });
    }
}
