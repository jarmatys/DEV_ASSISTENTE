using ASSISTENTE.Application.Abstractions;
using ASSISTENTE.Application.Abstractions.Clients;
using ASSISTENTE.Contract.Requests.Internal.Hub.UpdateQuestionFailed;
using ASSISTENTE.Contract.Requests.Internal.Hub.UpdateQuestionProgress;
using ASSISTENTE.Domain.Entities.Questions;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ASSISTENTE.Application.Handlers.Bases;

public abstract class QuestionCommandBase<TCommand>(
    ILogger logger,
    IAssistenteClientInternal clientInternal) : IRequestHandler<TCommand, Result>
    where TCommand : IRequest<Result>
{
    public async Task<Result> Handle(TCommand request, CancellationToken cancellationToken)
    {
        var connectionId = string.Empty;
        
        var result = await GetQuestionAsync(request)
            .ToResult(RepositoryErrors<Question>.NotFound.Build())
            .Bind(async question =>
            {
                connectionId = question.ConnectionId ?? string.Empty;
                
                return await UpdateProgress(question, InitialInformation)
                    .Bind(async () =>
                    {
                        logger.LogInformation(
                            "{Step} | ConnectionId: ({ConnectionId}) - '{Question}'",
                            typeof(TCommand).Name,
                            question.ConnectionId,
                            question.Text
                        );

                        return await HandleAsync(question);
                    })
                    .Bind(async () => await UpdateProgress(question, FinalInformation));
            });

        if (result.IsFailure)
        {
            await NotifyFail(connectionId);
        }
        
        return result;
    }

    protected abstract Task<Result> HandleAsync(Question question);
    protected abstract Task<Maybe<Question>> GetQuestionAsync(TCommand request);

    protected abstract ProgressInformation InitialInformation { get; }
    protected abstract ProgressInformation FinalInformation { get; }

    private async Task<Result> UpdateProgress(Question question, ProgressInformation information)
    {
        if (question.ConnectionId is null) return Result.Success();

        var request = UpdateQuestionProgressRequest.Create(
            question.ConnectionId,
            information.Progress,
            information.Text
        );
        
        return await clientInternal.UpdateQuestionProgressAsync(request);
    }
    
    private async Task<Result> NotifyFail(string? connectionId)
    {
        if (string.IsNullOrEmpty(connectionId)) return Result.Success();

        return await clientInternal
            .NotifyQuestionFailAsync(NotifyQuestionFailureRequest.Create(connectionId));
    }
}