using ASSISTENTE.Application.Abstractions;
using ASSISTENTE.Contract.Requests.Internal.Questions.Queries.GetQuestion;
using ASSISTENTE.Contract.Requests.Internal.Questions.Queries.GetQuestion.Models;
using ASSISTENTE.Domain.Entities.Questions;
using ASSISTENTE.Domain.Entities.Questions.Interfaces;
using ASSISTENTE.Language.Identifiers;
using CSharpFunctionalExtensions;
using MediatR;

namespace ASSISTENTE.Application.Handlers.Questions.Queries
{
    public sealed class GetQuestionQuery : IRequest<Result<GetQuestionResponse>>
    {
        private GetQuestionQuery(GetQuestionRequest request)
        {
            QuestionId = request.QuestionId;
        }
        
        
        public QuestionId QuestionId { get; }
        
        public static GetQuestionQuery Create(GetQuestionRequest request)
        {
            return new GetQuestionQuery(request);
        }
    }
    
    public class GetQuestionQueryHandler(IQuestionRepository questionRepository) 
        : IRequestHandler<GetQuestionQuery, Result<GetQuestionResponse>>
    {
        public async Task<Result<GetQuestionResponse>> Handle(GetQuestionQuery query, CancellationToken cancellationToken)
        {
            return await questionRepository.GetByIdAsync(query.QuestionId)
                .ToResult(RepositoryErrors<Question>.NotFound.Build())
                .Map(question =>
                {
                    var resources = question
                        .Resources
                        .Select(r => new ResourceDto(r.Resource.Id, r.Resource.Title))
                        .ToList();
                    
                    return new GetQuestionResponse(
                        question.Text, 
                        question.Answer!.Text, 
                        question.Context!.Value, 
                        resources);
                });
        }
    }
}
