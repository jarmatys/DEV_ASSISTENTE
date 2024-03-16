using ASSISTENTE.Domain.Interfaces;

namespace ASSISTENTE.Infrastructure.Services;

internal sealed class SystemTimeProvider : ISystemTimeProvider
{
    public DateTime Now()
    {
        return DateTime.Now;
    }
}