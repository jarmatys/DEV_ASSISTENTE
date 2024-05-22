using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ASSISTENTE.Persistence.Configuration;

public class DatabaseHealthCheck(IAssistenteDbContext dbContext) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken ct = new())
    {
        try
        {
            await dbContext.Resources.OrderBy(r => r.Created).FirstOrDefaultAsync(ct);

            return HealthCheckResult.Healthy("PostresSQL is available");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("PostresSQL is not available", ex);
        }
    }
}