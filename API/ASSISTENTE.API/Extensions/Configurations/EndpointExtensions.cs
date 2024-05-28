using ASSISTENTE.API.Parsers;
using ASSISTENTE.Common.Correlation;
using ASSISTENTE.Language.Identifiers;
using FastEndpoints;
using FastEndpoints.Swagger;

namespace ASSISTENTE.API.Extensions.Configurations;

internal static class EndpointExtensions
{
    internal static WebApplicationBuilder AddEndpoints(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddCorrelationProvider();

        builder.Services
            .AddFastEndpoints()
            .SwaggerDocument(options =>
            {
                options.ShortSchemaNames = true;
                options.DocumentSettings = settings =>
                {
                    settings.DocumentName =  "v1";
                    settings.Title = "Assistente API Documentation";
                    settings.Version = "1.0.0";
                };
            });

        return builder;
    }

    internal static WebApplication UseEndpoints(this WebApplication app)
    {
        app
            .UseFastEndpoints(c =>
            {
                c.RegisterIdentifierParsers();

                c.Endpoints.RoutePrefix = "api";
                c.Endpoints.ShortNames = true;

                c.Endpoints.Configurator = endpointDefinition =>
                {
                    endpointDefinition.AllowAnonymous();
                };
            })
            .UseSwaggerGen()
            .UseDefaultExceptionHandler();

        return app;
    }

    private static void RegisterIdentifierParsers(this Config c)
    {
        // TODO: Preare generic parsers for all identifiers
        c.Binding.ValueParserFor<QuestionId>(IdentifierParsers.GuidParser<QuestionId>);
        c.Binding.ValueParserFor<ResourceId>(IdentifierParsers.GuidParser<ResourceId>);
        c.Binding.ValueParserFor<AnswerId>(IdentifierParsers.NumberParser<AnswerId>);
        c.Binding.ValueParserFor<QuestionResourceId>(IdentifierParsers.NumberParser<QuestionResourceId>);
        c.Binding.ValueParserFor<QuestionFileId>(IdentifierParsers.NumberParser<QuestionFileId>);
    }
}