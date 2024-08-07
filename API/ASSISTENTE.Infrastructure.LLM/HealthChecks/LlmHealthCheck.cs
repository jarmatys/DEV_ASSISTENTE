using ASSISTENTE.Infrastructure.LLM.Contracts;
using CSharpFunctionalExtensions;
using SOFTURE.Common.HealthCheck.Core;

namespace ASSISTENTE.Infrastructure.LLM.HealthChecks;

internal class LlmHealthCheck(ILlmClient llmClient) : CheckBase
{
    protected override async Task<Result> Check()
    {
        return await Prompt.Create("Test")
            .Bind(llmClient.GenerateAnswer);
    }
}