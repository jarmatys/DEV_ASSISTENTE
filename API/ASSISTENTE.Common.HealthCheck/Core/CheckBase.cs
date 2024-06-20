using CSharpFunctionalExtensions;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ASSISTENTE.Common.HealthCheck.Core;

public abstract class CheckBase() : ICommonHealthCheck
{
    private string Name => GetType().Name.Replace("HealthCheck", string.Empty);

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
                ? HealthCheckResult.Healthy($"{Name} is available")
                : HealthCheckResult.Unhealthy($"{Name} is not available", data: data);
        }
        catch (Exception ex)
        {
            data.Add("Exception", ex.InnerException?.Message ?? ex.Message);

            return HealthCheckResult.Unhealthy($"{Name} is not available", data: data);
        }
    }
}