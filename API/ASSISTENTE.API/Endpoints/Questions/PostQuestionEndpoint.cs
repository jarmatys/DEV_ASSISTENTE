using ASSISTENTE.Application.Knowledge.Commands.ResolveQuestionContext;
using ASSISTENTE.Contract.Requests.Internal.Knowledge.Commands.ResolveQuestionContext;
using MediatR;

namespace ASSISTENTE.API.Endpoints.Questions;

public sealed class PostQuestionEndpoint(ISender mediator)
    : EndpointBase<ResolveQuestionContextRequest, ResolveQuestionContextCommand>(mediator)
{
    public override void Configure()
    {
        Post("/api/questions");
        AllowAnonymous();
        SetupSwagger();
    }

    protected override ResolveQuestionContextCommand MediatRequest(ResolveQuestionContextRequest req, CancellationToken ct)
        => ResolveQuestionContextCommand.Create(req);
}