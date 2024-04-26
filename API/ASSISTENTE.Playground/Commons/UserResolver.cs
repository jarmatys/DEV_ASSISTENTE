using ASSISTENTE.Domain.Interfaces;

namespace ASSISTENTE.Playground.Commons;

internal sealed class UserResolver : IUserResolver
{
    public string GetUserEmail()
    {
        return "PLAYGROUND";
    }
}