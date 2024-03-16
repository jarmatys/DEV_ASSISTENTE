using ASSISTENTE.Domain.Interfaces;

namespace ASSISTENTE.Client.Commons;

internal abstract class UserResolver : IUserResolver
{
    public string GetUserEmail()
    {
        return "Playground";
    }
}