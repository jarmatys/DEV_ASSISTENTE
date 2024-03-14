﻿using ASSISTENTE.Infrastructure.CodeParser;
using ASSISTENTE.Infrastructure.Embeddings;
using ASSISTENTE.Infrastructure.Interfaces;
using ASSISTENTE.Infrastructure.MarkDownParser;
using ASSISTENTE.Infrastructure.Qdrant;
using ASSISTENTE.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ASSISTENTE.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMarkDownParser();
            services.AddCodeParser();
            services.AddEmbeddings(configuration);
            services.AddQdrant(configuration);
            
            services.AddScoped<IKnowledgeService, KnowledgeService>();
            services.AddScoped<IFileParser, FileParser>();
            services.AddScoped<IMaintenanceService, MaintenanceService>();
            
            return services;
        }
    }
}
