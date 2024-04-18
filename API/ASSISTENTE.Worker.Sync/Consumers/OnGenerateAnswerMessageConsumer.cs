using ASSISTENTE.Contract.Internal.Messages.Knowledge;
using ASSISTENTE.Worker.Sync.Bases;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Worker.Sync.Consumers;

public sealed class OnGenerateAnswerMessageConsumer : ResultConsumerBase<GenerateAnswerMessage>
{
    protected override Task<Result> ConsumeAsync()
    {
        throw new NotImplementedException();
    }
}