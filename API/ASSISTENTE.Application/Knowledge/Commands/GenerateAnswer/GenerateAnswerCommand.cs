using ASSISTENTE.Application.Abstractions;
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
        IQuestionRepository questionRepository,
        ILogger<GenerateAnswerCommandHandler> logger)
        : IRequestHandler<GenerateAnswerCommand, Result>
    {
        public async Task<Result> Handle(GenerateAnswerCommand request, CancellationToken cancellationToken)
        {
            // TODO: Call to API with status

            return await questionRepository.GetByIdAsync(request.QuestionId)
                .ToResult(RepositoryErrors.NotFound.Build())
                .Bind(question =>
                {
                    logger.LogInformation(
                        "3 | ConnectionId: ({ConnectionId}) - '{Question}' answering question...",
                        question.ConnectionId,
                        question.Text
                    );

                    // TODO: Extract logic to generate answer
                    
                    return Result.Success();
                });
        }
    }
}