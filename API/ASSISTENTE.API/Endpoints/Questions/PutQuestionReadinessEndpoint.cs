using ASSISTENTE.API.Hubs;
using ASSISTENTE.Contract.Requests.Internal.Hub.NotifyQuestionReadiness;
using FastEndpoints;
using Microsoft.AspNetCore.SignalR;

namespace ASSISTENTE.API.Endpoints.Questions;

public sealed class PutQuestionReadinessEndpoint(IHubContext<QuestionHub, IQuestionHub> hubContext) : Endpoint<NotifyQuestionReadinessRequest> 
{
    public override void Configure()
    {
        Put("questions/readiness");
                
        // TODO: add request validation
        // Validator<MyValidator>();
    }

    public override async Task HandleAsync(NotifyQuestionReadinessRequest req, CancellationToken ct)
    {
        await hubContext.Clients.Client(req.ConnectionId).NotifyReadiness(req.QuestionId);
        
        await SendOkAsync(ct);
    }
}