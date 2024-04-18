using ASSISTENTE.Common.Settings;

namespace ASSISTENTE.Worker.Sync.Common;

public sealed class WorkerSettings
{
    public required SeqSection Seq { get; init; }
    public required RabbitSection Rabbit { get; init; }
}