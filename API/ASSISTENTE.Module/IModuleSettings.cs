using ASSISTENTE.Client.Internal.Settings;
using ASSISTENTE.Infrastructure.Embeddings.Settings;
using ASSISTENTE.Infrastructure.Firecrawl.Settings;
using ASSISTENTE.Infrastructure.Langfuse.Settings;
using ASSISTENTE.Infrastructure.LLM.Settings;
using ASSISTENTE.Infrastructure.Qdrant.Settings;
using ASSISTENTE.Persistence.Configuration.Settings;

namespace ASSISTENTE.Module;

public interface IModuleSettings : 
    IInternalApiSettings, 
    IDatabaseSettings, 
    IQdrantSettings, 
    IEmbeddingsSettings, 
    ILlmSettings,
    IFirecrawlSettings,
    ILangfuseSettings
{
}