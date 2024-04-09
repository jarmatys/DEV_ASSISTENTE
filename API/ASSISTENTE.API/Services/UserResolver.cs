using ASSISTENTE.Domain.Interfaces;

namespace ASSISTENTE.API.Services;

internal sealed class UserResolver : IUserResolver
{
    public string GetUserEmail()
    {
        return "API";
    }
}