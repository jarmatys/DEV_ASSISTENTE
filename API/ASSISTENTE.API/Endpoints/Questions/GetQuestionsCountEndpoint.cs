using ASSISTENTE.Application.Handlers.Questions.Queries;
using ASSISTENTE.Contract.Requests.Internal.Questions.Queries.GetQuestionsCount;
using MediatR;

namespace ASSISTENTE.API.Endpoints.Questions;

public sealed class GetQuestionsCountEndpoint(ISender mediator)
    : QueryEndpointBase<GetQuestionsCountResponse, GetQuestionsCountQuery>(mediator)
{
    public override void Configure()
    {
        Get("questions/count");
        SetupSwagger();
    }

    protected override GetQuestionsCountQuery MediatRequest() 
        => GetQuestionsCountQuery.Create();
}