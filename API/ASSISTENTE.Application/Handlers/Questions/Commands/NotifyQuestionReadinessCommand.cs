using ASSISTENTE.Application.Abstractions;
using ASSISTENTE.Application.Abstractions.Clients;
using ASSISTENTE.Contract.Requests.Internal.Hub.NotifyQuestionReadiness;
using ASSISTENTE.Domain.Entities.Questions;
using ASSISTENTE.Domain.Entities.Questions.Interfaces;
using ASSISTENTE.Language.Identifiers;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ASSISTENTE.Application.Handlers.Questions.Commands
{
    public sealed class NotifyQuestionReadinessCommand : IRequest<Result>
    {
        private NotifyQuestionReadinessCommand(QuestionId questionId)
        {
            QuestionId = questionId;
        }

        public QuestionId QuestionId { get; }

        public static NotifyQuestionReadinessCommand Create(QuestionId questionId)
        {
            return new NotifyQuestionReadinessCommand(questionId);
        }
    }

    public class NotifyQuestionReadinessCommandHandler(
        IQuestionRepository questionRepository,
        IAssistenteClientInternal clientInternal,
        ILogger<NotifyQuestionReadinessCommandHandler> logger)
        : IRequestHandler<NotifyQuestionReadinessCommand, Result>
    {
        public async Task<Result> Handle(NotifyQuestionReadinessCommand request, CancellationToken cancellationToken)
        {
            return await questionRepository.GetByIdAsync(request.QuestionId)
                .ToResult(RepositoryErrors<Question>.NotFound.Build())
                .Bind(async question =>
                {
                    logger.LogInformation(
                        "{Step} | ConnectionId: ({ConnectionId}) - '{Question}'",
                        nameof(NotifyQuestionReadinessCommand),
                        question.ConnectionId,
                        question.Text
                    );
                    
                    if (question.ConnectionId is null) return Result.Success();

                    var requestBody = NotifyQuestionReadinessRequest.Create(
                        question.ConnectionId,
                        question.Id
                    );

                    return await clientInternal.NotifyQuestionReadinessAsync(requestBody);
                });
        }
    }
}