using ASSISTENTE.Client.Internal.Settings;
using ASSISTENTE.Infrastructure.Firecrawl.Settings;
using ASSISTENTE.Infrastructure.Langfuse.Settings;
using ASSISTENTE.Infrastructure.LLM.Ollama.Settings;
using ASSISTENTE.Infrastructure.LLM.OpenAi.Settings;
using ASSISTENTE.Infrastructure.Qdrant.Settings;
using ASSISTENTE.Persistence.Configuration.Settings;

namespace ASSISTENTE.Module;

public interface IModuleSettings : 
    IInternalApiSettings, 
    IDatabaseSettings, 
    IQdrantSettings, 
    IOpenAiSettings, 
    IOllamaSettings,
    IFirecrawlSettings,
    ILangfuseSettings
{
}