using ASSISTENTE.API.Common.Parsers;
using ASSISTENTE.Language;
using FastEndpoints;
using FastEndpoints.Swagger;

namespace ASSISTENTE.API.Common.Extensions;

internal static class EndpointExtensions
{
    internal static WebApplicationBuilder AddEndpoints(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();

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

                c.Endpoints.Configurator = config =>
                {
                    // Configuration to apply for all endpoints
                };
            })
            .UseSwaggerGen()
            .UseDefaultExceptionHandler();

        return app;
    }

    private static void RegisterIdentifierParsers(this Config config)
    {
        var identifierTypes = typeof(IIdentifier).Assembly.GetTypes()
            .Where(t => t.IsClass && typeof(IIdentifier).IsAssignableFrom(t) && !t.IsAbstract)
            .ToList();
        
        foreach (var identifierType in identifierTypes)
        {
            var valueType = identifierType.BaseType?.GetGenericArguments().FirstOrDefault();
            if (valueType == typeof(Guid))
            {
                config.RegisterParser(identifierType, nameof(IdentifierParsers.GuidParser));
            }
            else if (valueType == typeof(int))
            {
                config.RegisterParser(identifierType, nameof(IdentifierParsers.NumberParser));
            }
        }
    }
    
    private static void RegisterParser(this Config c, Type identifierType, string parserMethodName)
    {
        var method = typeof(IdentifierParsers).GetMethod(parserMethodName)?.MakeGenericMethod(identifierType);
        var delegateType = typeof(Func<,>).MakeGenericType(typeof(object), typeof(ParseResult));
        var parserDelegate = Delegate.CreateDelegate(delegateType, method!);

        var bindingMethod = c.Binding.GetType()
            .GetMethod("ValueParserFor", [delegateType])
            ?.MakeGenericMethod(identifierType);
        
        bindingMethod?.Invoke(c.Binding, [parserDelegate]);
    }
}