namespace Infrastructure.JobStorage;

public class JobDbContext(DbContextOptions o) : DbContext(o)
{
    public DbSet<JobRecord> Jobs { get; set; }
}
