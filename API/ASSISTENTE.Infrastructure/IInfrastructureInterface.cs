using ASSISTENTE.Infrastructure.Firecrawl.Settings;
using ASSISTENTE.Infrastructure.Langfuse.Settings;
using ASSISTENTE.Infrastructure.LLM.Ollama.Settings;
using ASSISTENTE.Infrastructure.LLM.OpenAi.Settings;
using ASSISTENTE.Infrastructure.Neo4J.Settings;
using ASSISTENTE.Infrastructure.Pdf4Me.Settings;
using ASSISTENTE.Infrastructure.Qdrant.Settings;

namespace ASSISTENTE.Infrastructure;

public interface IInfrastructureInterface :
    IOpenAiSettings, 
    IQdrantSettings, 
    IFirecrawlSettings, 
    ILangfuseSettings, 
    IOllamaSettings, 
    INeo4JSettings, 
    IPdf4MeSettings;