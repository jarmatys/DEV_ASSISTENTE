using ASSISTENTE.Application.Abstractions;
using ASSISTENTE.Application.Abstractions.Clients;
using ASSISTENTE.Application.Abstractions.Interfaces;
using ASSISTENTE.Contract.Requests.Internal.Hub.UpdateQuestion;
using ASSISTENTE.Domain.Entities.Questions;
using ASSISTENTE.Domain.Entities.Questions.Interfaces;
using ASSISTENTE.Language.Identifiers;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ASSISTENTE.Application.Questions.Commands.FindResources
{
    public sealed class FindResourcesCommand : IRequest<Result>
    {
        private FindResourcesCommand(QuestionId questionId)
        {
            QuestionId = questionId;
        }

        public QuestionId QuestionId { get; }

        public static FindResourcesCommand Create(QuestionId questionId)
        {
            return new FindResourcesCommand(questionId);
        }
    }

    public class FindResourcesCommandHandler(
        IKnowledgeService knowledgeService,
        IQuestionRepository questionRepository,
        IAssistenteClientInternal clientInternal,
        ILogger<FindResourcesCommandHandler> logger)
        : IRequestHandler<FindResourcesCommand, Result>
    {
        public async Task<Result> Handle(FindResourcesCommand request, CancellationToken cancellationToken)
        {
            // TODO: Extract command base class
            return await questionRepository.GetByIdAsync(request.QuestionId)
                .ToResult(RepositoryErrors<Question>.NotFound.Build())
                .Bind(async question =>
                {
                    return await UpdateStatus(question.ConnectionId!, QuestionProgress.Started) // TODO: call if connectionId is null
                        .Bind(async () =>
                        {
                            logger.LogInformation(
                                "3 | ConnectionId: ({ConnectionId}) - '{Question}' searching for resources...",
                                question.ConnectionId,
                                question.Text
                            );

                            return await knowledgeService.FindResources(question);
                        })
                        .Bind(async () => await UpdateStatus(question.ConnectionId!, QuestionProgress.ResourcesFound));
                });
        }

        private async Task<Result> UpdateStatus(string connectionId, QuestionProgress progress)
            => await clientInternal.UpdateQuestionAsync(UpdateQuestionRequest.Create(connectionId, progress));
    }
}