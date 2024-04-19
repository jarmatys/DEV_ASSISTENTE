using ASSISTENTE.Common.Messaging;
using ASSISTENTE.Worker.Sync.Common.Exceptions;
using CSharpFunctionalExtensions;
using MassTransit;

namespace ASSISTENTE.Worker.Sync.Common.Bases;

public abstract class ResultConsumerBase<TMessage> : IConsumer<TMessage>
    where TMessage : class, IMessage
{
    public async Task Consume(ConsumeContext<TMessage> context)
    {
        var consumeResult = await ConsumeAsync();

        if (!consumeResult.IsSuccess)
        {
            throw new ConsumeException(consumeResult.Error);
        }
    }

    protected abstract Task<Result> ConsumeAsync();
}