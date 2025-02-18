namespace Infrastructure.JobStorage;

public sealed class MongoDbSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;
}
