using ASSISTENTE.Application.Handlers.Questions.Queries;
using ASSISTENTE.Contract.Requests.Internal.Questions.Queries.GetAnswer;
using MediatR;

namespace ASSISTENTE.API.Endpoints.Answers;

public sealed class GetAnswerEndpoint(ISender mediator)
    : QueryEndpointBase<GetAnswerRequest, GetAnswerResponse, GetAnswerQuery>(mediator)
{
    public override void Configure()
    {
        Get("answers/{@Id}", x => new { Id = x.QuestionId });
        SetupSwagger();
    }

    protected override GetAnswerQuery MediatRequest(GetAnswerRequest req) 
        => GetAnswerQuery.Create(req);
}