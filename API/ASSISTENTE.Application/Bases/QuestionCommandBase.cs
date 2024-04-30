using ASSISTENTE.Application.Abstractions;
using ASSISTENTE.Application.Abstractions.Clients;
using ASSISTENTE.Contract.Requests.Internal.Hub.UpdateQuestionProgress;
using ASSISTENTE.Domain.Entities.Questions;
using ASSISTENTE.Language.Enums;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ASSISTENTE.Application.Bases;

public abstract class QuestionCommandBase<TCommand>(
    ILogger logger,
    IAssistenteClientInternal clientInternal) : IRequestHandler<TCommand, Result>
    where TCommand : IRequest<Result>
{
    public async Task<Result> Handle(TCommand request, CancellationToken cancellationToken)
    {
        return await GetQuestionAsync(request)
            .ToResult(RepositoryErrors<Question>.NotFound.Build())
            .Bind(async question =>
            {
                return await UpdateProgress(question, InitialProgress)
                    .Bind(async () =>
                    {
                        logger.LogInformation(
                            "{StepNumber} | ConnectionId: ({ConnectionId}) - '{Question}'",
                            InitialProgress,
                            question.ConnectionId,
                            question.Text
                        );

                        return await HandleAsync(question);
                    })
                    .Bind(async () => await UpdateProgress(question, FinalProgress));
            });
    }

    protected abstract Task<Result> HandleAsync(Question question);
    protected abstract Task<Maybe<Question>> GetQuestionAsync(TCommand request);

    protected abstract QuestionProgress InitialProgress { get; }
    protected abstract QuestionProgress FinalProgress { get; }

    private async Task<Result> UpdateProgress(Question question, QuestionProgress progress)
    {
        if (question.ConnectionId is null) return Result.Success();

        return await clientInternal
            .UpdateQuestionProgressAsync(UpdateQuestionProgressRequest.Create(question.ConnectionId, progress));
    }
}