using ASSISTENTE.Domain.Interfaces;

namespace ASSISTENTE.Client.Commons;

internal sealed class UserResolver : IUserResolver
{
    public string GetUserEmail()
    {
        return "Playground";
    }
}