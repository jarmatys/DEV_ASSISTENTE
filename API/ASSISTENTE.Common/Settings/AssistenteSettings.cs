using ASSISTENTE.Common.Settings.Sections;

namespace ASSISTENTE.Common.Settings;

public sealed class AssistenteSettings
{
    public required DatabaseSection Database { get; set; }
    public required OpenAiSection OpenAi { get; set; }
    public required SeqSection Seq { get; set; }
    public required QdrantSection Qdrant { get; set; }
    public required RabbitSection Rabbit { get; set; }
    public required InternalApiSection InternalApi { get; set; }
    public required OpenTelemetrySection OpenTelemetry { get; set; }
}