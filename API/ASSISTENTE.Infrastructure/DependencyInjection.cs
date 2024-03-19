using ASSISTENTE.Common.Extensions;
using ASSISTENTE.Domain.Interfaces;
using ASSISTENTE.Infrastructure.CodeParser;
using ASSISTENTE.Infrastructure.Embeddings;
using ASSISTENTE.Infrastructure.Interfaces;
using ASSISTENTE.Infrastructure.LLM;
using ASSISTENTE.Infrastructure.MarkDownParser;
using ASSISTENTE.Infrastructure.Options;
using ASSISTENTE.Infrastructure.PromptGenerator;
using ASSISTENTE.Infrastructure.Qdrant;
using ASSISTENTE.Infrastructure.Services;
using ASSISTENTE.Infrastructure.Services.Parsers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ASSISTENTE.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddMarkDownParser();
            services.AddCodeParser();
            services.AddEmbeddings(configuration);
            services.AddQdrant(configuration);
            services.AddPromptGenerator();
            services.AddLLM(configuration);

            services.AddScoped<IKnowledgeService, KnowledgeService>();
            services.AddScoped<IFileParser, FileParser>();
            services.AddScoped<IMaintenanceService, MaintenanceService>();
            services.AddScoped<ISystemTimeProvider, SystemTimeProvider>();

            services.AddOption<KnowledgePathsOption>(configuration, "KnowledgePaths");

            return services;
        }
    }
}