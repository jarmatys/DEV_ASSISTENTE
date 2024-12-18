using ASSISTENTE.Application.Abstractions.Interfaces;
using ASSISTENTE.Domain.Interfaces;
using ASSISTENTE.Infrastructure.CodeParser;
using ASSISTENTE.Infrastructure.Embeddings;
using ASSISTENTE.Infrastructure.Embeddings.Settings;
using ASSISTENTE.Infrastructure.Firecrawl;
using ASSISTENTE.Infrastructure.Firecrawl.Settings;
using ASSISTENTE.Infrastructure.Langfuse;
using ASSISTENTE.Infrastructure.Langfuse.Settings;
using ASSISTENTE.Infrastructure.LLM;
using ASSISTENTE.Infrastructure.LLM.Settings;
using ASSISTENTE.Infrastructure.MarkDownParser;
using ASSISTENTE.Infrastructure.PromptGenerator;
using ASSISTENTE.Infrastructure.Qdrant;
using ASSISTENTE.Infrastructure.Qdrant.Settings;
using ASSISTENTE.Infrastructure.Services;
using ASSISTENTE.Infrastructure.Services.Parsers;
using Microsoft.Extensions.DependencyInjection;

namespace ASSISTENTE.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure<TSettings>(this IServiceCollection services)
            where TSettings : IEmbeddingsSettings, ILlmSettings, IQdrantSettings, IFirecrawlSettings, ILangfuseSettings
        {
            services.AddMarkDownParser();
            services.AddCodeParser();
            services.AddEmbeddings<TSettings>();
            services.AddQdrant<TSettings>();
            services.AddPromptGenerator();
            services.AddLlm<TSettings>();
            services.AddFirecrawl<TSettings>();
            services.AddLangfuse<TSettings>();

            services.AddScoped<IKnowledgeService, KnowledgeService>();
            services.AddScoped<IQuestionOrchestrator, QuestionOrchestrator>();
            services.AddScoped<IFileParser, FileParser>();
            services.AddScoped<IMaintenanceService, MaintenanceService>();
            services.AddScoped<ISystemTimeProvider, SystemTimeProvider>();
            
            return services;
        }
    }
}