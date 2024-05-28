using ASSISTENTE.Domain.Interfaces;

namespace ASSISTENTE.API.Common.Services;

internal sealed class UserResolver : IUserResolver
{
    public string GetUserEmail()
    {
        return "API";
    }
}