using ASSISTENTE.Infrastructure.PromptGenerator.Enums;
using ASSISTENTE.Infrastructure.PromptGenerator.Interfaces;

namespace ASSISTENTE.Infrastructure.PromptGenerator.Prompts;

internal class FilePrompt : IPrompt
{
    public PromptType Type => PromptType.Files;

    public string Generate(string question, IEnumerable<string> context)
    {
        var prompt = $"""
                      You have obtained the following requirement for implementation:

                      ### Requirement
                      {question}
                      ###
                      
                      -----------------------------

                      Based on the file list, select most appropriate files and answer the question as best as you can and truthfully.

                      ### files list
                      {string.Join("\n", context)}
                      ###
                      
                      ### Example
                      
                      Files list:
                      FilePrompt.cs
                      IQuestionOrchestrator.cs
                      RepositoryErrors.cs
                      
                      Answer:
                      FilePrompt.cs
                      
                      --------------------------
                      
                      IMPORTANT: Return only selected file names without other information.
                      """;

        return prompt;
    }
}