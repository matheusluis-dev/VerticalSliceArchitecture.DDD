namespace Infrastructure;

using System;
using FastEndpoints;

public sealed class JobRecord : IJobStorageRecord
{
    public Guid TrackingID { get; set; } = Guid.NewGuid();
    public string QueueID { get; set; } = default!;
    public object Command { get; set; } = default!;
    public DateTime ExecuteAfter { get; set; }
    public DateTime ExpireOn { get; set; }
    public bool IsComplete { get; set; }
    public bool IsCancelled { get; set; }
}
