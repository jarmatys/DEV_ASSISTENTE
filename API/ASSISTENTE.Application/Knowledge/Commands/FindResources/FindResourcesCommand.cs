using ASSISTENTE.Application.Abstractions;
using ASSISTENTE.Application.Abstractions.Interfaces;
using ASSISTENTE.Domain.Entities.Questions.Interfaces;
using ASSISTENTE.Language.Identifiers;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ASSISTENTE.Application.Knowledge.Commands.FindResources
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
        ILogger<FindResourcesCommandHandler> logger)
        : IRequestHandler<FindResourcesCommand, Result>
    {
        public async Task<Result> Handle(FindResourcesCommand request, CancellationToken cancellationToken)
        {
            // TODO: Call to API with status
            
            return await questionRepository.GetByIdAsync(request.QuestionId)
                .ToResult(RepositoryErrors.NotFound.Build())
                .Bind(async question =>
                {
                    logger.LogInformation(
                        "2 | ConnectionId: ({ConnectionId}) - '{Question}' searching for resources...",
                        question.ConnectionId,
                        question.Text
                    );
            
                    return await knowledgeService.FindResources(question);
                })
                .Tap(_ =>
                {
                    // TODO: Call to API with status
                });
        }
    }
}