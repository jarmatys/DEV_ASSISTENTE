using ASSISTENTE.Application.Handlers.Questions.Commands;
using ASSISTENTE.Contract.Messages.Internal.Knowledge;
using ASSISTENTE.Worker.Sync.Common.Bases;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ASSISTENTE.Worker.Sync.Consumers;

public sealed class OnResolveContextMessageConsumer(
    ILogger<OnResolveContextMessageConsumer> logger, 
    ISender mediator) : ResultConsumerBase<ResolveContextMessage, ResolveContextCommand>(logger, mediator)
{
    protected override ResolveContextCommand MediatRequest(ResolveContextMessage message)
        => ResolveContextCommand.Create(message.QuestionId);
}