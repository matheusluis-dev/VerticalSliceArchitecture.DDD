namespace Infrastructure.JobStorage;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using FastEndpoints;
using Infrastructure.Services;
using MongoDB.Driver;

public sealed class JobStorage : IJobStorageProvider<JobRecord>
{
    private readonly IMongoCollection<JobRecord> _jobs;
    private readonly IDateTimeService _dateTime;

    public JobStorage(IMongoDatabase mongo, IDateTimeService dateTime)
    {
        ArgumentNullException.ThrowIfNull(mongo);

        _jobs = mongo.GetCollection<JobRecord>("Jobs");
        _dateTime = dateTime;
    }

    public Task StoreJobAsync(JobRecord r, CancellationToken ct)
    {
        return _jobs.InsertOneAsync(r, cancellationToken: ct);
    }

    public async Task<IEnumerable<JobRecord>> GetNextBatchAsync(PendingJobSearchParams<JobRecord> parameters)
    {
        var jobs = await _jobs.FindAsync(parameters.Match, cancellationToken: parameters.CancellationToken);

        return await jobs.ToListAsync(parameters.CancellationToken);
    }

    public Task MarkJobAsCompleteAsync(JobRecord r, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(r);

        r.IsComplete = true;
        return _jobs.ReplaceOneAsync(j => j.TrackingID == r.TrackingID, r, cancellationToken: ct);
    }

    public async Task CancelJobAsync(Guid trackingId, CancellationToken ct)
    {
        var job = await _jobs.Find(j => j.TrackingID == trackingId).FirstOrDefaultAsync(ct);

        job.IsComplete = true;
        await _jobs.ReplaceOneAsync(j => j.TrackingID == job.TrackingID, job, cancellationToken: ct);
    }

    public Task OnHandlerExecutionFailureAsync([NotNull] JobRecord r, Exception exception, CancellationToken ct)
    {
        r.ExecuteAfter = _dateTime.UtcNow.DateTime.AddMinutes(1);
        return _jobs.ReplaceOneAsync(j => j.TrackingID == r.TrackingID, r, cancellationToken: ct);
    }

    public Task PurgeStaleJobsAsync(StaleJobSearchParams<JobRecord> parameters)
    {
        return _jobs.DeleteManyAsync(parameters.Match, cancellationToken: parameters.CancellationToken);
    }
}
