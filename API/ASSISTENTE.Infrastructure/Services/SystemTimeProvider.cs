using ASSISTENTE.Domain.Interfaces;

namespace ASSISTENTE.Infrastructure.Services;

internal abstract class SystemTimeProvider : ISystemTimeProvider
{
    public DateTime Now()
    {
        return DateTime.Now;
    }
}