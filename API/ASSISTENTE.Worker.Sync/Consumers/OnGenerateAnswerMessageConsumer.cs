using ASSISTENTE.Contract.Internal.Messages.Knowledge;
using MassTransit;

namespace ASSISTENTE.Worker.Sync.Consumers;

// TODO: prepare base class for consumers
public sealed class OnGenerateAnswerMessageConsumer : IConsumer<GenerateAnswerMessage> 
{
    public Task Consume(ConsumeContext<GenerateAnswerMessage> context)
    {
        throw new NotImplementedException();
    }
}