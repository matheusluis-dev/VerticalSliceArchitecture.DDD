namespace Infrastructure.Job;

public sealed class MongoDbSettings
{
    public string ConnectionString { get; init; } = null!;

    public string DatabaseName { get; init; } = null!;
}
