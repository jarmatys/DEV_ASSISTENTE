using ASSISTENTE.Application.Questions.Commands.ResolveContext;
using ASSISTENTE.Contract.Messages.Internal.Knowledge;
using ASSISTENTE.Worker.Sync.Common.Bases;
using MediatR;

namespace ASSISTENTE.Worker.Sync.Consumers;

public sealed class OnResolveContextMessageConsumer(ISender mediator)
    : ResultConsumerBase<ResolveContextMessage, ResolveContextCommand>(mediator)
{
    protected override ResolveContextCommand MediatRequest(ResolveContextMessage message)
        => ResolveContextCommand.Create(message.QuestionId);
}