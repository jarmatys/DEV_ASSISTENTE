using ASSISTENTE.Client.Internal.Settings;
using ASSISTENTE.Infrastructure.Embeddings.Settings;
using ASSISTENTE.Infrastructure.Firecrawl.Settings;
using ASSISTENTE.Infrastructure.LLM.Settings;
using ASSISTENTE.Infrastructure.Qdrant.Settings;
using ASSISTENTE.Module;
using ASSISTENTE.Persistence.Configuration.Settings;
using SOFTURE.Common.Logging.Settings;

namespace ASSISTENTE.Playground;

internal sealed class PlaygroundSettings : IModuleSettings, ISeqSettings
{
    public required InternalApiSettings InternalApi { get; init; }
    public required SeqSettings Seq { get; init; }
    public required QdrantSettings Qdrant { get; init; }
    public required DatabaseSettings Database { get; init; }
    public required LlmSettings Llm { get; init; }
    public required EmbeddingsSettings Embeddings { get; init; }
    public required FirecrawlSettings Firecrawl { get; init; }
}