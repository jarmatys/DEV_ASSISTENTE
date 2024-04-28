using ASSISTENTE.Application.Abstractions;
using ASSISTENTE.Application.Abstractions.Clients;
using ASSISTENTE.Application.Abstractions.Interfaces;
using ASSISTENTE.Contract.Requests.Internal.Hub.UpdateQuestion;
using ASSISTENTE.Domain.Entities.Questions.Interfaces;
using ASSISTENTE.Language.Identifiers;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ASSISTENTE.Application.Questions.Commands.ResolveContext
{
    public sealed class ResolveContextCommand : IRequest<Result>
    {
        private ResolveContextCommand(QuestionId questionId)
        {
            QuestionId = questionId;
        }
        
        public QuestionId QuestionId { get; }
        
        public static ResolveContextCommand Create(QuestionId questionId)
        {
            return new ResolveContextCommand(questionId);
        }
    }

    public class ResolveQuestionCommandHandler(
        IKnowledgeService knowledgeService,
        IQuestionRepository questionRepository,
        IAssistenteClientInternal clientInternal,
        ILogger<ResolveQuestionCommandHandler> logger)
        : IRequestHandler<ResolveContextCommand, Result>
    {
        public async Task<Result> Handle(ResolveContextCommand request, CancellationToken cancellationToken)
        {
            return await questionRepository.GetByIdAsync(request.QuestionId)
                .ToResult(RepositoryErrors.NotFound.Build())
                .Bind(async question =>
                {
                    return await UpdateStatus(question.ConnectionId!, QuestionProgress.ResolvingContext)
                        .Bind(async () =>
                        {
                            logger.LogInformation(
                                "2 | (ConnectionId: {ConnectionId}) - '{Question}' resolving question context...",
                                question.ConnectionId,
                                question.Text
                            );

                            return await knowledgeService.ResolveContext(question);
                        })
                        .Bind(async () => await UpdateStatus(question.ConnectionId!, QuestionProgress.ContextResolved));
                });
        }
        
        private async Task<Result> UpdateStatus(string connectionId, QuestionProgress progress)
            => await clientInternal.UpdateQuestionAsync(UpdateQuestionRequest.Create(connectionId, progress));
    }
}