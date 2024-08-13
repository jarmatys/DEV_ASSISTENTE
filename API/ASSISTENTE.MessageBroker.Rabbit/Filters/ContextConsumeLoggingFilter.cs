using MassTransit;
using SOFTURE.Common.Correlation.Consts;
using SOFTURE.Common.Correlation.Generators;
using SOFTURE.Common.Correlation.Providers;
using SOFTURE.Common.Correlation.ValueObjects;
using LogContext = Serilog.Context.LogContext;

namespace ASSISTENTE.MessageBroker.Rabbit.Filters;

public class ContextConsumeLoggingFilter<T>(ICorrelationProvider correlationProvider) : IFilter<ConsumeContext<T>>
    where T : class
{
    public void Probe(ProbeContext context)
    {
        context.CreateFilterScope(nameof(ContextConsumeLoggingFilter<T>));
    }

    public async Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
    {
        var correlationId = GetCorrelationId(context) ?? CorrelationGenerator.Generate();
        
        correlationProvider.Set(CorrelationId.Parse(correlationId));

        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            await next.Send(context);
        }
    }
    
    private static string? GetCorrelationId(ConsumeContext context)
    {
        context.Headers.TryGetHeader(CorrelationConsts.CorrelationHeader, out var correlationId);

        return correlationId?.ToString();
    }
}