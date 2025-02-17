namespace Infrastructure;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Infrastructure.Persistence;

public sealed class JobProvider : IJobStorageProvider<JobRecord>
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<JobRecord> _set;

    public JobProvider(ApplicationDbContext context)
    {
        _context = context;
        _set = _context.Set<JobRecord>();
    }

    public async Task CancelJobAsync(Guid trackingId, CancellationToken ct)
    {
        var job = await _set.FindAsync([trackingId], ct);

        job.IsComplete = true;

        _set.Update(job);
        await _context.SaveChangesAsync(ct);
    }

    public Task<IEnumerable<JobRecord>> GetNextBatchAsync(
        PendingJobSearchParams<JobRecord> parameters
    )
    {
        return Task.FromResult(_set.Where(parameters.Match).Take(parameters.Limit).AsEnumerable());
    }

    public async Task MarkJobAsCompleteAsync([NotNull] JobRecord r, CancellationToken ct)
    {
        r.IsComplete = true;

        _set.Update(r);
        await _context.SaveChangesAsync(ct);
    }

    public async Task OnHandlerExecutionFailureAsync(
        JobRecord r,
        Exception exception,
        CancellationToken ct
    )
    {
        r.ExecuteAfter = DateTime.Now.AddMinutes(5);

        _set.Update(r);
        await _context.SaveChangesAsync(ct);
    }

    public Task PurgeStaleJobsAsync(StaleJobSearchParams<JobRecord> parameters)
    {
        return _set.Where(parameters.Match).ExecuteDeleteAsync(parameters.CancellationToken);
    }

    public async Task StoreJobAsync(JobRecord r, CancellationToken ct)
    {
        await _set.AddAsync(r, ct);
        await _context.SaveChangesAsync(ct);
    }
}
