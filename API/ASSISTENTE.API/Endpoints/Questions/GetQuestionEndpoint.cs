using ASSISTENTE.Application.Questions.Queries.GetQuestion;
using ASSISTENTE.Contract.Requests.Internal.Questions.Queries.GetQuestion;
using MediatR;

namespace ASSISTENTE.API.Endpoints.Questions;

public sealed class GetQuestionEndpoint(ISender mediator)
    : QueryEndpointBase<GetQuestionRequest, GetQuestionResponse, GetQuestionQuery>(mediator)
{
    public override void Configure()
    {
        Get("/api/questions/{@Id}", x => new { Id = x.QuestionId });
        AllowAnonymous();
        SetupSwagger();
    }
    
    protected override GetQuestionQuery MediatRequest(GetQuestionRequest req)
        => GetQuestionQuery.Create(req);
}