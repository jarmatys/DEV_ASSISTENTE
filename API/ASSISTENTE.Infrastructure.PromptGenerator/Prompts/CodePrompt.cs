using ASSISTENTE.Infrastructure.PromptGenerator.Enums;
using ASSISTENTE.Infrastructure.PromptGenerator.Interfaces;

namespace ASSISTENTE.Infrastructure.PromptGenerator.Prompts;

internal class CodePrompt : IPrompt
{
    public PromptType Type => PromptType.Code;

    public string Generate(string question, IEnumerable<string> context)
    {
        var prompt = $"""
                      You have obtained the following requirement for implementation:

                      ### Requirement
                      {question}
                      ###
                      
                      -----------------------------

                      Based on the code fragments below, prepare solution proposals, answer them as best as you can and truthfully.

                      ### code fragments
                      {string.Join("\n", context)}
                      ###
                      """;

        return prompt;
    }
}