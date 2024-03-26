namespace ASSISTENTE.Infrastructure.PromptGenerator;

public class SourceProvider : ISourceProvider
{
    public string Prompt<T>(string question) where T : struct, Enum
    {
        var availableOptions = string.Join(", ", Enum.GetValues<T>());

        var promptText = $"""
                          You have the following resource types available: "{availableOptions}".

                          Based on the question "{question}", select the appropriate resource type.

                          ###
                          Example:

                          Q: Generate some code for me
                          A: Code

                          Q: Remind me what I should do if I want to configure a database
                          A: Note

                          Q: {question}
                          A:
                          """;

        return promptText;
    }
}