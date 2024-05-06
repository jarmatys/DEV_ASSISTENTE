using ASSISTENTE.Application.Handlers.Questions.Queries;
using ASSISTENTE.Contract.Requests.Internal.Questions.Queries.GetQuestion;
using MediatR;

namespace ASSISTENTE.API.Endpoints.Questions;

public sealed class GetQuestionEndpoint(ISender mediator)
    : QueryEndpointBase<GetQuestionRequest, GetQuestionResponse, GetQuestionQuery>(mediator)
{
    public override void Configure()
    {
        Get("questions/{@Id}", x => new { Id = x.QuestionId });
        SetupSwagger();
        
        Summary(x =>
        {
            x.RequestParam(r => r.QuestionId, "Question identifier");
        });
    }
    
    protected override GetQuestionQuery MediatRequest(GetQuestionRequest req)
        => GetQuestionQuery.Create(req);
}