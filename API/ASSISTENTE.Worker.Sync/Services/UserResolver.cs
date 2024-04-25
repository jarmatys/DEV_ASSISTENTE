using ASSISTENTE.Domain.Interfaces;

namespace ASSISTENTE.Worker.Sync.Services;

internal sealed class UserResolver : IUserResolver
{
    public string GetUserEmail()
    {
        return "WORKER.SYNC";
    }
}