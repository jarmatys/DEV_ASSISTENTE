using CSharpFunctionalExtensions;
using OpenAI;
using SOFTURE.Common.HealthCheck.Core;

namespace ASSISTENTE.Infrastructure.LLM.OpenAi.HealthChecks;

internal class OpenAiHealthCheck(OpenAIClient client) : CheckBase
{
    protected override async Task<Result> Check()
    {
        try
        {
            await client.ModelsEndpoint.GetModelsAsync();

            return Result.Success();
        }
        catch (Exception exception)
        {
            return Result.Failure($"Open AI is down: {exception.Message}");
        }
    }
}