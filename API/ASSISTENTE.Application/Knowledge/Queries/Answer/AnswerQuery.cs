using ASSISTENTE.Contract.Internal.Knowledge.Queries.Answer;
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
    
    public class AnswerQueryHandler : IRequestHandler<AnswerQuery, Result<AnswerResponse>>
    {
        public async Task<Result<AnswerResponse>> Handle(AnswerQuery query, CancellationToken cancellationToken)
        {
            return Result.Success(new AnswerResponse());
        }
    }
}
