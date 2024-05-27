using MassTransit;
using System.Diagnostics;
using LogContext = Serilog.Context.LogContext;

namespace ASSISTENTE.Worker.Sync.Common.Filters;

public class ContextConsumeLoggingFilter : IFilter<ConsumeContext>
{
    public void Probe(ProbeContext context)
    {
        context.CreateFilterScope(nameof(ContextConsumeLoggingFilter));
    }

    public async Task Send(ConsumeContext context, IPipe<ConsumeContext> next)
    {
        var correlationId = GetCorrelationId(context);

        LogContext.PushProperty("CorrelationId", correlationId);

        await next.Send(context);
    }

    private static string? GetCorrelationId(ConsumeContext context) => Activity.Current?.RootId;
}