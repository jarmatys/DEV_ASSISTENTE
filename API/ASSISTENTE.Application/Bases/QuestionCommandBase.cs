using ASSISTENTE.Application.Abstractions;
using ASSISTENTE.Application.Abstractions.Clients;
using ASSISTENTE.Contract.Requests.Internal.Hub.UpdateQuestion;
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
                return await UpdateStatus(question, InitialProgress)
                    .Bind(async () =>
                    {
                        logger.LogInformation(
                            "{StepNumber} | ConnectionId: ({ConnectionId}) - '{Question}' {Action}...",
                            InitialProgress,
                            question.ConnectionId,
                            question.Text,
                            InitialProgress.ToString()
                        );

                        return await HandleAsync(question);
                    })
                    .Bind(async () => await UpdateStatus(question, FinalProgress));
            });
    }

    protected abstract Task<Result> HandleAsync(Question question);
    protected abstract Task<Maybe<Question>> GetQuestionAsync(TCommand request);

    protected abstract QuestionProgress InitialProgress { get; }
    protected abstract QuestionProgress FinalProgress { get; }

    private async Task<Result> UpdateStatus(Question question, QuestionProgress progress)
    {
        if (question.ConnectionId is null) return Result.Success();

        return await clientInternal.UpdateQuestionAsync(UpdateQuestionRequest.Create(question.ConnectionId, progress));
    }
}