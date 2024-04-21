using ASSISTENTE.Application.Knowledge.Queries.GenerateAnswer;
using ASSISTENTE.Contract.Internal.Requests.Knowledge.Queries.GenerateAnswer;
using MediatR;

namespace ASSISTENTE.API.Endpoints.Answers;

public sealed class PostAnswerEndpoint(ISender mediator)
    : EndpointBase<GenerateAnswerRequest, GenerateAnswerResponse, GenerateAnswerQuery>(mediator)
{
    public override void Configure()
    {
        Post("/api/answer");
        AllowAnonymous();
        SetupSwagger();
    }

    protected override GenerateAnswerQuery MediatRequest(GenerateAnswerRequest req, CancellationToken ct)
        => GenerateAnswerQuery.Create(req.Question);
}