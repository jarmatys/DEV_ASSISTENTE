using ASSISTENTE.API.Hubs;
using ASSISTENTE.Contract.Requests.Internal.Hub.UpdateQuestionFailed;
using FastEndpoints;
using Microsoft.AspNetCore.SignalR;

namespace ASSISTENTE.API.Endpoints.Questions;

public sealed class PutQuestionFailureEndpoint(IHubContext<QuestionHub, IQuestionHub> hubContext) 
    : Endpoint<NotifyQuestionFailureRequest> 
{
    public override void Configure()
    {
        Put("questions/failure");
        AllowAnonymous();
    }

    public override async Task HandleAsync(NotifyQuestionFailureRequest req, CancellationToken ct)
    {
        await hubContext.Clients.Client(req.ConnectionId).NotifyFailure();
        
        await SendOkAsync(ct);
    }
}