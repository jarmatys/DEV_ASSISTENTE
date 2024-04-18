using ASSISTENTE.Common.Messaging;
using ASSISTENTE.Worker.Sync.Exceptions;
using CSharpFunctionalExtensions;
using MassTransit;

namespace ASSISTENTE.Worker.Sync.Bases;

public abstract class ResultConsumerBase<TMessage> : IConsumer<TMessage>
    where TMessage : class, IMessage
{
    public async Task Consume(ConsumeContext<TMessage> context)
    {
        var consumeResult = await ConsumeAsync();

        if (!consumeResult.IsSuccess)
        {
            throw new WorkerException(consumeResult.Error);
        }
    }

    protected abstract Task<Result> ConsumeAsync();
}