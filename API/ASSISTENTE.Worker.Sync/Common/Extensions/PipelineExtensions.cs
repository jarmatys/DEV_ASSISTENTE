using ASSISTENTE.Worker.Sync.Common.Filters;
using MassTransit;

namespace ASSISTENTE.Worker.Sync.Common.Extensions;

internal static class PipelineExtensions
{
    public static void UsePipelineFilters(this IConsumePipeConfigurator configurator)
    {
        configurator.UseFilter(new ContextConsumeLoggingFilter());
    }
}