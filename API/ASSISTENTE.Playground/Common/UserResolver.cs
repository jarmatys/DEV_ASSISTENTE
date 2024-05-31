using ASSISTENTE.Domain.Interfaces;

namespace ASSISTENTE.Playground.Common;

internal sealed class UserResolver : IUserResolver
{
    public string GetUserEmail()
    {
        return "PLAYGROUND";
    }
}