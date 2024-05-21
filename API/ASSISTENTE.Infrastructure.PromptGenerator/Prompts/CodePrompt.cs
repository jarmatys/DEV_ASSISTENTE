using ASSISTENTE.Infrastructure.PromptGenerator.Enums;
using ASSISTENTE.Infrastructure.PromptGenerator.Interfaces;

namespace ASSISTENTE.Infrastructure.PromptGenerator.Prompts;

internal class CodePrompt : IPrompt
{
    public PromptType Type => PromptType.Code;

    public string Generate(string question, string context)
    {
        var prompt = $"""
                      You have obtained the following requirement for implementation:

                      ### Requirement
                      {question}
                      ###
                      
                      -----------------------------

                      Based on the code fragments below, prepare solution proposals, answer them as best as you can and truthfully. 
                      In solution show ready code fragments that can be used to solve the requirement.

                      ### code fragments
                      {context}
                      ###
                      """;

        return prompt;
    }
}