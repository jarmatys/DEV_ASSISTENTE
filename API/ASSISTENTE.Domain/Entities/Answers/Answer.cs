using ASSISTENTE.Domain.Commons;
using ASSISTENTE.Domain.Entities.Questions;

namespace ASSISTENTE.Domain.Entities.Answers;

public sealed class Answer : AuditableEntity<int>
{
    private Answer()
    {
    }

    private Answer(string text, string prompt, string client, string model, int promptTokens, int completionTokens)
    {
        Text = text;
        Prompt = prompt;
        Client = client;
        Model = model;
        PromptTokens = promptTokens;
        CompletionTokens = completionTokens;

        Question = null!;
    }

    public string Text { get; private set; } = null!;
    public string Prompt { get; private set; } = null!;
    public string Client { get; private set; } = null!;
    public string Model { get; private set; } = null!;
    public int PromptTokens { get; private set; }
    public int CompletionTokens { get; private set; }

    # region NAVIGATION PROPERTIES

    public Guid QuestionId { get; private set; }
    public Question Question { get; private set; } = null!;

    # endregion

    public static Result<Answer> Create(
        string text,
        string prompt,
        string client,
        string model,
        int promptTokens,
        int completionTokens)
    {
        return new Answer(text, prompt, client, model, promptTokens, completionTokens);
    }
}