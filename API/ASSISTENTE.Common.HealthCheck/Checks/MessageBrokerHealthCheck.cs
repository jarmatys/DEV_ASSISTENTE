using CSharpFunctionalExtensions;
using MassTransit;

namespace ASSISTENTE.Common.HealthCheck.Checks;

internal class MessageBrokerHealthCheck(IPublishEndpoint publishEndpoint) : CheckBase("Message broker")
{
    protected override async Task<Result> Check()
    {
        await publishEndpoint.Publish(new HealthCheckMessage(), Cts.Token);
        
        return Result.Success();
    }

    private record HealthCheckMessage;
}