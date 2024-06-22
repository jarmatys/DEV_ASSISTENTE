using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;
using Polly;
using Polly.CircuitBreaker;
using Polly.Fallback;
using Polly.Retry;

namespace ASSISTENTE.Common.Resilience;

public static class DependencyInjection
{
    public const string RetryPipeline = "retry";

    public static IServiceCollection AddCommonResilience(this IServiceCollection services)
    {
        // INJECT: ResiliencePipelineProvider<string> pipelineProvider
        // var pipeline = pipelineProvider.GetPipeline<HttpResponseMessage>(RetryPipeline);
        // var response = await pipeline.ExecuteAsync(async token => await httpClient.GetAsync("...", token), ct);
        
        services.AddResiliencePipeline<string, HttpResponseMessage>(RetryPipeline, pipelineBuilder =>
        {
            pipelineBuilder.AddHedging(new HttpHedgingStrategyOptions
            {
                Delay = TimeSpan.FromSeconds(1),
            });

            pipelineBuilder.AddFallback(new FallbackStrategyOptions<HttpResponseMessage>
            {
                FallbackAction = _ => Outcome.FromResultAsValueTask(new HttpResponseMessage(HttpStatusCode.OK)),
                OnFallback = async _ => await Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)),
            });

            pipelineBuilder.AddRetry(new RetryStrategyOptions<HttpResponseMessage>
            {
                Delay = TimeSpan.FromSeconds(1),
                MaxRetryAttempts = 2,
                BackoffType = DelayBackoffType.Exponential,
                UseJitter = true
            });

            pipelineBuilder.AddCircuitBreaker(new CircuitBreakerStrategyOptions<HttpResponseMessage>
            {
                BreakDuration = TimeSpan.FromSeconds(30)
            });
        });

        return services;
    }
}