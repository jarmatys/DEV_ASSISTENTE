using CSharpFunctionalExtensions;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ASSISTENTE.Common.HealthCheck;

internal abstract class CheckBase(string name) : IHealthCheck
{
    protected readonly CancellationTokenSource Cts = new(TimeSpan.FromSeconds(2));

    protected abstract Task<Result> Check();

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken ct = new())
    {
        var data = new Dictionary<string, object>();

        try
        {
            var result = await Check();

            if (result.IsFailure)
                data.Add("Error", result.Error);

            return result.IsSuccess
                ? HealthCheckResult.Healthy($"{name} is available")
                : HealthCheckResult.Unhealthy($"{name} is not available", data: data);
        }
        catch (Exception ex)
        {
            data.Add("Exception", ex.InnerException?.Message ?? ex.Message);
            
            return HealthCheckResult.Unhealthy($"{name} is not available", data: data);
        }
    }
}