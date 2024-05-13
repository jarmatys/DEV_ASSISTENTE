using ASSISTENTE.Application.Handlers.Questions.Commands;
using ASSISTENTE.Contract.Requests.Internal.Questions.Commands.CreateQuestion;
using MediatR;

namespace ASSISTENTE.API.Endpoints.Questions;

public sealed class PostQuestionCommandEndpoint(ISender mediator)
    : CommandEndpointBase<CreateQuestionRequest, CreateQuestionCommand>(mediator)
{
    public override void Configure()
    {
        Post("questions");
        SetupSwagger();
        Options(x => x.RequireRateLimiting("limiterPolicy"));
    }

    protected override CreateQuestionCommand MediatRequest(CreateQuestionRequest req) 
        => CreateQuestionCommand.Create(req);
}