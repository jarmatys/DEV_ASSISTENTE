using ASSISTENTE.Client.Internal.Settings;
using ASSISTENTE.Infrastructure.Embeddings.Settings;
using ASSISTENTE.Infrastructure.LLM.Settings;
using ASSISTENTE.Infrastructure.Qdrant.Settings;
using ASSISTENTE.Module;
using ASSISTENTE.Persistence.Configuration.Settings;
using SOFTURE.Common.Authentication.Settings;
using SOFTURE.Common.Logging.Settings;
using SOFTURE.Common.Observability.Settings;
using SOFTURE.MessageBroker.Rabbit.Settings;

namespace ASSISTENTE.API;

internal sealed class ApiSettings : 
    IModuleSettings, 
    ISeqSettings, 
    IRabbitSettings, 
    IObservabilitySettings, 
    IAuthenticationSettings
{
    public required InternalApiSettings InternalApi { get; init; }
    public required SeqSettings Seq { get; init; }
    public required QdrantSettings Qdrant { get; init; }
    public required DatabaseSettings Database { get; init; }
    public required LlmSettings Llm { get; init; }
    public required EmbeddingsSettings Embeddings { get; init; }
    public required RabbitSettings Rabbit { get; init; }
    public required ObservabilitySettings Observability { get; init; }
    public required AuthenticationSettings Authentication { get; init; }
}