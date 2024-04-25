using ASSISTENTE.Common.Messaging;
using ASSISTENTE.Worker.Sync.Common.Exceptions;
using CSharpFunctionalExtensions;
using MassTransit;
using MediatR;

namespace ASSISTENTE.Worker.Sync.Common.Bases;

public abstract class ResultConsumerBase<TMessage, TMediatRequest>(ISender mediator) : IConsumer<TMessage>
    where TMessage : class, IMessage
    where TMediatRequest : IRequest<Result>
{
    public async Task Consume(ConsumeContext<TMessage> context)
    {
        var mediatRequest = MediatRequest(context.Message);

        await mediator.Send(mediatRequest)
            .Tap(() => Task.CompletedTask) // TODO: log success
            .TapError(errorMessage => throw new ConsumeException(errorMessage)); // TODO: Log error
    }
    
    protected abstract TMediatRequest MediatRequest(TMessage message);

}