using ASSISTENTE.Application.Questions.Commands.CreateQuestion;
using ASSISTENTE.Contract.Requests.Internal.Questions.Commands.CreateQuestion;
using MediatR;

namespace ASSISTENTE.API.Endpoints.Questions;

public sealed class PostQuestionEndpoint(ISender mediator)
    : EndpointBase<CreateQuestionRequest, CreateQuestionCommand>(mediator)
{
    public override void Configure()
    {
        Post("/api/questions");
        AllowAnonymous();
        SetupSwagger();
    }

    protected override CreateQuestionCommand MediatRequest(CreateQuestionRequest req, CancellationToken ct)
        => CreateQuestionCommand.Create(req);
}