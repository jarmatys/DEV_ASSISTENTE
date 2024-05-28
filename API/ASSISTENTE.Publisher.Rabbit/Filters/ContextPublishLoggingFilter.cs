using ASSISTENTE.Common.Correlation.Consts;
using ASSISTENTE.Common.Correlation.Providers;
using MassTransit;

namespace ASSISTENTE.Publisher.Rabbit.Filters;

public class ContextPublishLoggingFilter<T>(ICorrelationProvider correlationProvider) : IFilter<PublishContext<T>>
    where T : class
{
    public void Probe(ProbeContext context)
    {
        context.CreateFilterScope(nameof(ContextPublishLoggingFilter<T>));
    }

    public async Task Send(PublishContext<T> context, IPipe<PublishContext<T>> next)
    {
        var correlationId = correlationProvider.Get();
        if (correlationId is not null)
        {
            context.Headers.Set(CorrelationConsts.CorrelationHeader, correlationId);
        }
        
        await next.Send(context);
    }
}