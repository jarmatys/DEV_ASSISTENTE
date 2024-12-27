using ASSISTENTE.Domain.Interfaces;

namespace ASSISTENTE.API.Agent;

internal sealed class UserResolver : IUserResolver
{
    public string GetUserEmail()
    {
        return "AGENT API";
    }
}