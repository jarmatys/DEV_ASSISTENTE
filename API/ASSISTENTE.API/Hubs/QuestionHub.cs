using ASSISTENTE.Language.Identifiers;
using Microsoft.AspNetCore.SignalR;

namespace ASSISTENTE.API.Hubs;

public sealed class QuestionHub : Hub<IQuestionHub>
{
    public override async Task OnConnectedAsync()
    {
        await Clients.Client(Context.ConnectionId).InitConnection(Context.ConnectionId);
            
        await base.OnConnectedAsync();
    }
}

public interface IQuestionHub
{
    Task InitConnection(string connectionId);
    Task ReceiveProgress(int progress, string information);
    Task NotifyReadiness(QuestionId questionId);
    Task NotifyFailure();
}