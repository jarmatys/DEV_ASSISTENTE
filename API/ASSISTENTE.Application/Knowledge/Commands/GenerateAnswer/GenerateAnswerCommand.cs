using ASSISTENTE.Application.Abstractions;
using ASSISTENTE.Application.Abstractions.Clients;
using ASSISTENTE.Application.Abstractions.Interfaces;
using ASSISTENTE.Contract.Requests.Internal.Knowledge.Commands.UpdateQuestion;
using ASSISTENTE.Domain.Entities.Questions.Interfaces;
using ASSISTENTE.Language.Identifiers;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ASSISTENTE.Application.Knowledge.Commands.GenerateAnswer
{
    public sealed class GenerateAnswerCommand : IRequest<Result>
    {
        private GenerateAnswerCommand(QuestionId questionId)
        {
            QuestionId = questionId;
        }

        public QuestionId QuestionId { get; }

        public static GenerateAnswerCommand Create(QuestionId questionId)
        {
            return new GenerateAnswerCommand(questionId);
        }
    }

    public class GenerateAnswerCommandHandler(
        IKnowledgeService knowledgeService,
        IQuestionRepository questionRepository,
        IAssistenteClientInternal clientInternal,
        ILogger<GenerateAnswerCommandHandler> logger)
        : IRequestHandler<GenerateAnswerCommand, Result>
    {
        public async Task<Result> Handle(GenerateAnswerCommand request, CancellationToken cancellationToken)
        {
            return await questionRepository.GetByIdAsync(request.QuestionId)
                .ToResult(RepositoryErrors.NotFound.Build())
                .Bind(async question =>
                {
                    return await UpdateStatus(question.ConnectionId!, QuestionProgress.Answering)
                        .Bind(async () =>
                        {
                            logger.LogInformation(
                                "3 | ConnectionId: ({ConnectionId}) - '{Question}' answering question...",
                                question.ConnectionId,
                                question.Text
                            );

                            return await knowledgeService.GenerateAnswer(question);
                        })
                        .Bind(async _ => await UpdateStatus(question.ConnectionId!, QuestionProgress.Answered));
                });
        }

        private async Task<Result> UpdateStatus(string connectionId, QuestionProgress progress)
            => await clientInternal.UpdateQuestionAsync(UpdateQuestionRequest.Create(connectionId, progress));
    }
}