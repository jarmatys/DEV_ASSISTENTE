using ASSISTENTE.Application.Knowledge.Queries.Answer;
using ASSISTENTE.Contract.Internal.Knowledge.Queries.Answer;
using CSharpFunctionalExtensions;
using MediatR;

namespace ASSISTENTE.API.Endpoints.Answers;

public sealed class AnswerEndpoint(ISender mediator) : EndpointBase<AnswerRequest, AnswerResponse>(mediator)
{
    public override void Configure()
    {
        Post("/api/answer");
        AllowAnonymous();
    }

    protected override async Task<Result<AnswerResponse>> HandleResultAsync(AnswerRequest req, CancellationToken ct)
        => await Mediator.Send(AnswerQuery.Create(req.Question), ct);
}