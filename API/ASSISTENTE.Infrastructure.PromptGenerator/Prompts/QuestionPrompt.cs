using ASSISTENTE.Infrastructure.PromptGenerator.Enums;
using ASSISTENTE.Infrastructure.PromptGenerator.Interfaces;

namespace ASSISTENTE.Infrastructure.PromptGenerator.Prompts;

internal class QuestionPrompt : IPrompt
{
    public PromptType Type => PromptType.Question;

    public string Generate(string question, IEnumerable<string> context)
    {
        var prompt = $"""
                      You received the following question:

                      ### question
                      {question}
                      ###
                      
                      -----------------------------

                      Based on the context below, answer them as best as you can and truthfully.

                      ### context
                      {string.Join("\n", context)}
                      ###
                      """;

        return prompt;
    }
}