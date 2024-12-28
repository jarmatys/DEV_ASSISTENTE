using ASSISTENTE.Application.Abstractions.Interfaces;
using ASSISTENTE.Domain.Interfaces;
using ASSISTENTE.Infrastructure.Audio;
using ASSISTENTE.Infrastructure.CodeParser;
using ASSISTENTE.Infrastructure.Embeddings;
using ASSISTENTE.Infrastructure.Enums;
using ASSISTENTE.Infrastructure.Firecrawl;
using ASSISTENTE.Infrastructure.Neo4J;
using ASSISTENTE.Infrastructure.Image;
using ASSISTENTE.Infrastructure.Langfuse;
using ASSISTENTE.Infrastructure.Pdf4Me;
using ASSISTENTE.Infrastructure.LLM;
using ASSISTENTE.Infrastructure.MarkDownParser;
using ASSISTENTE.Infrastructure.PromptGenerator;
using ASSISTENTE.Infrastructure.Qdrant;
using ASSISTENTE.Infrastructure.Services;
using ASSISTENTE.Infrastructure.Services.Parsers;
using ASSISTENTE.Infrastructure.Vision;
using Microsoft.Extensions.DependencyInjection;

namespace ASSISTENTE.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure<TSettings>(
            this IServiceCollection services,
            PrivacyMode privacyMode)
            where TSettings : IInfrastructureInterface
        {
            services.AddMarkDownParser();
            services.AddCodeParser();
            services.AddQdrant<TSettings>();
            services.AddPromptGenerator();
            services.AddFirecrawl<TSettings>();
            services.AddLangfuse<TSettings>();
            services.AddNeo4J<TSettings>();
            services.AddPdf4Me<TSettings>();

            if (privacyMode == PrivacyMode.Cloud)
            {
                services.AddOpenAiLlm<TSettings>();
                services.AddOpenAiEmbeddings<TSettings>();
                services.AddOpenAiAudio<TSettings>();
                services.AddOpenAiVision<TSettings>();
                services.AddOpenAiImage<TSettings>();
            }

            if (privacyMode == PrivacyMode.Local)
            {
                services.AddOllamaLlm<TSettings>();
                services.AddOllamaEmbeddings<TSettings>();
            }

            services.AddScoped<IKnowledgeService, KnowledgeService>();
            services.AddScoped<IQuestionOrchestrator, QuestionOrchestrator>();
            services.AddScoped<IFileParser, FileParser>();
            services.AddScoped<IMaintenanceService, MaintenanceService>();
            services.AddScoped<ISystemTimeProvider, SystemTimeProvider>();

            return services;
        }
    }
}