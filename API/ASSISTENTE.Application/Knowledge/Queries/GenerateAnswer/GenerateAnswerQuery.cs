using ASSISTENTE.Application.Abstractions.Interfaces;
using ASSISTENTE.Contract.Internal.Requests.Knowledge.Queries.GenerateAnswer;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ASSISTENTE.Application.Knowledge.Queries.GenerateAnswer
{
    public sealed class GenerateAnswerQuery : IRequest<Result<GenerateAnswerResponse>>
    {
        private GenerateAnswerQuery(string question)
        {
            Question = question;
        }
        
        public string Question { get; }
        
        public static GenerateAnswerQuery Create(string question)
        {
            return new GenerateAnswerQuery(question);
        }
    }
    
    public class GenerateAnswerQueryHandler(IKnowledgeService knowledgeService, ILogger<GenerateAnswerQueryHandler> logger) 
        : IRequestHandler<GenerateAnswerQuery, Result<GenerateAnswerResponse>>
    {
        public async Task<Result<GenerateAnswerResponse>> Handle(GenerateAnswerQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("Question: '{Question}' is being answered...", query.Question);
            
            return await knowledgeService.RecallAsync(query.Question, connectionId: null)
                .Map(text => new GenerateAnswerResponse(text));
        }
    }
}
