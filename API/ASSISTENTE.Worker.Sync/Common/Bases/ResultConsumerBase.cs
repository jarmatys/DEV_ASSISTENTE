using System.Threading.Tasks;
using ASSISTENTE.Worker.Sync.Common.Exceptions;
using CSharpFunctionalExtensions;
using MassTransit;
using MediatR;
using SOFTURE.Contract.Common.Messaging;

namespace ASSISTENTE.Worker.Sync.Common.Bases;

public abstract class ResultConsumerBase<TMessage, TMediatRequest>(
    ILogger logger,
    ISender mediator) : IConsumer<TMessage>
    where TMessage : class, IMessage
    where TMediatRequest : IRequest<Result>
{
    public async Task Consume(ConsumeContext<TMessage> context)
    {
        AddTrace(context, "CONSUME_START");

        var mediatRequest = MediatRequest(context.Message);

        await mediator.Send(mediatRequest)
            .Tap(() => AddTrace(context, "CONSUME_SUCCESS"))
            .TapError(errorMessage =>
            {
                AddTrace(context, "CONSUME_FAILED");

                throw new ConsumeException(errorMessage);
            });
    }

    protected abstract TMediatRequest MediatRequest(TMessage message);

    private void AddTrace(ConsumeContext<TMessage> context, string state)
    {
        logger.LogTrace(
            "{MessageId}|{State}|{MessageType}|{@Payload}",
            context.MessageId,
            state,
            typeof(TMessage).Name,
            context.Message
        );
    }
}