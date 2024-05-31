using ASSISTENTE.Application.Abstractions.Interfaces;
using ASSISTENTE.Common.Settings;
using ASSISTENTE.Domain.Interfaces;
using ASSISTENTE.Infrastructure.CodeParser;
using ASSISTENTE.Infrastructure.Embeddings;
using ASSISTENTE.Infrastructure.LLM;
using ASSISTENTE.Infrastructure.MarkDownParser;
using ASSISTENTE.Infrastructure.PromptGenerator;
using ASSISTENTE.Infrastructure.Qdrant;
using ASSISTENTE.Infrastructure.Services;
using ASSISTENTE.Infrastructure.Services.Parsers;
using Microsoft.Extensions.DependencyInjection;

namespace ASSISTENTE.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            AssistenteSettings settings)
        {
            services.AddMarkDownParser();
            services.AddCodeParser();
            services.AddEmbeddings(settings.OpenAi);
            services.AddQdrant(settings.Qdrant);
            services.AddPromptGenerator();
            services.AddLlm(settings.OpenAi);

            services.AddScoped<IKnowledgeService, KnowledgeService>();
            services.AddScoped<IQuestionOrchestrator, QuestionOrchestrator>();
            services.AddScoped<IFileParser, FileParser>();
            services.AddScoped<IMaintenanceService, MaintenanceService>();
            services.AddScoped<ISystemTimeProvider, SystemTimeProvider>();
            
            return services;
        }
    }
}