using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using FastEndpoints;

namespace Infrastructure.Job;

public sealed class JobRecord : IJobStorageRecord
{
    public string QueueID { get; set; } = null!;
    public Guid TrackingID { get; set; }
    public DateTime ExecuteAfter { get; set; }

    public DateTime ExpireOn { get; set; }
    public bool IsComplete { get; set; }

    [NotMapped]
    public object Command { get; set; } = null!;

    private string CommandJson { get; set; } = null!;

    TCommand IJobStorageRecord.GetCommand<TCommand>()
    {
        return JsonSerializer.Deserialize<TCommand>(CommandJson)!;
    }

    void IJobStorageRecord.SetCommand<TCommand>(TCommand command)
    {
        CommandJson = JsonSerializer.Serialize(command);
    }
}
