
using ASSISTENTE.Common.HealthCheck;
using ASSISTENTE.Common.HealthCheck.Core;
using CSharpFunctionalExtensions;
using MassTransit;

namespace ASSISTENTE.MessageBroker.Rabbit.HealthChecks;

internal class MessageBrokerHealthCheck(IPublishEndpoint publishEndpoint) : CheckBase
{
    protected override async Task<Result> Check()
    {
        await publishEndpoint.Publish(new HealthCheckMessage(), Cts.Token);
        
        return Result.Success();
    }

    private record HealthCheckMessage;
}