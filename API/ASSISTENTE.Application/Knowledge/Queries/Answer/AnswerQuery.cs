using ASSISTENTE.Contract.Internal.Knowledge.Queries.Answer;
using ASSISTENTE.Infrastructure.Interfaces;
using CSharpFunctionalExtensions;
using MediatR;

namespace ASSISTENTE.Application.Knowledge.Queries.Answer
{
    public sealed class AnswerQuery : IRequest<Result<AnswerResponse>>
    {
        private AnswerQuery(string question)
        {
            Question = question;
        }
        
        public string Question { get; }
        
        public static AnswerQuery Create(string question)
        {
            return new AnswerQuery(question);
        }
    }
    
    public class AnswerQueryHandler(IKnowledgeService knowledgeService) 
        : IRequestHandler<AnswerQuery, Result<AnswerResponse>>
    {
        public async Task<Result<AnswerResponse>> Handle(AnswerQuery query, CancellationToken cancellationToken)
        {
            // TODO: Add serilog and use logger instead of Console.WriteLine

            Console.WriteLine($"\nQuestion: '{query.Question}'");
            
            return await knowledgeService.RecallAsync(query.Question)
                .Map(AnswerResponse.Create);
        }
    }
}
