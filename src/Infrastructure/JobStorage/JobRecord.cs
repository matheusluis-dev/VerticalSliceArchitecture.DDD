namespace Infrastructure.JobStorage;

using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using FastEndpoints;

public sealed class JobRecord : IJobStorageRecord
{
    public Guid Id { get; set; }
    public string QueueID { get; set; } = default!;
    public Guid TrackingID { get; set; }
    public DateTime ExecuteAfter { get; set; }
    public DateTime ExpireOn { get; set; }
    public bool IsComplete { get; set; }

    [NotMapped]
    public object Command { get; set; } = default!;

    public string CommandJson { get; set; } = default!;

    TCommand IJobStorageRecord.GetCommand<TCommand>()
    {
        return JsonSerializer.Deserialize<TCommand>(CommandJson)!;
    }

    void IJobStorageRecord.SetCommand<TCommand>(TCommand command)
    {
        CommandJson = JsonSerializer.Serialize(command);
    }

    [NotMapped]
    public object? Result { get; set; }

    public string? ResultJson { get; set; }
}
