using ASSISTENTE.Infrastructure.LLM;
using ASSISTENTE.Infrastructure.LLM.ValueObjects;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Common.HealthCheck.Checks;

internal class LlmHealthCheck(ILLMClient llmClient) : CheckBase("LLM")
{
    protected override async Task<Result> Check()
    {
        return await Prompt.Create("Test")
            .Bind(llmClient.GenerateAnswer);
    }
}