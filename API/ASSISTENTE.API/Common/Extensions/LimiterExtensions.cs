using System.Text.Json;
using FastEndpoints;
using FluentValidation.Results;
using Microsoft.AspNetCore.RateLimiting;

namespace ASSISTENTE.API.Common.Extensions;

internal static class LimiterExtensions
{
    private const string RateLimitErrorKey = "RateLimit";

    private const string RateLimitError = "Request rate for today exceeded. " +
                                          "Please try again tomorrow or check history in 'Questions' section."; 
   
    private const int StatusCode = 429;
    
    internal static WebApplicationBuilder AddLimiter(this WebApplicationBuilder builder)
    {
        builder.Services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCode;
            options.OnRejected = async (OnRejectedContext context, CancellationToken token) =>
            {
                var error = new ErrorResponse([new ValidationFailure(RateLimitErrorKey, RateLimitError)], StatusCode);
               
                var json = JsonSerializer.Serialize(error);
                
                await context.HttpContext.Response.WriteAsync(json, cancellationToken: token);
            };
            options.AddFixedWindowLimiter(
                policyName: "limiterPolicy", fixedOptions =>
                {
                    fixedOptions.PermitLimit = 25;
                    fixedOptions.Window = TimeSpan.FromDays(1);
                    fixedOptions.QueueLimit = 0;
                });
        });

        return builder;
    }
    
    internal static WebApplication UseLimiter(this WebApplication app)
    {
        app.UseRateLimiter();

        return app;
    }
}