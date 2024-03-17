using ASSISTENTE.Infrastructure.PromptGenerator.Enums;
using ASSISTENTE.Infrastructure.PromptGenerator.Interfaces;

namespace ASSISTENTE.Infrastructure.PromptGenerator.Prompts;

internal class QuestionPrompt : IPrompt
{
    public PromptType Type => PromptType.Question;

    public string Generate(string question, string context)
    {
        var prompt = $"""
                      You received the following question:

                      {question}

                      -----------------------------

                      Based on the context below, answer them as best as you can and truthfully.

                      ### context
                      
                      {context}

                      ###
                      """;

        return prompt;
    }
}