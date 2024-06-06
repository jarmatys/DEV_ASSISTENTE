using ASSISTENTE.Domain.Common;
using ASSISTENTE.Domain.Entities.Answers.ValueObjects;
using ASSISTENTE.Domain.Entities.Questions;
using ASSISTENTE.Language.Identifiers;

namespace ASSISTENTE.Domain.Entities.Answers;

public sealed class Answer : AuditableEntity<AnswerId>
{
    private Answer()
    {
    }

    private Answer(string text, string prompt, LlmMetadata metadata)
    {
        Text = text;
        Prompt = prompt;
        Metadata = metadata;
    }

    public string Text { get; private set; } = null!;
    public string Prompt { get; private set; } = null!;
    public LlmMetadata Metadata { get; private set; } = null!;

    # region NAVIGATION PROPERTIES

    public QuestionId QuestionId { get; private set; } = null!;
    public Question Question { get; private set; } = null!;

    # endregion

    internal static Result<Answer> Create(
        string text,
        string prompt,
        LlmMetadata metadata)
    {
        return new Answer(text, prompt, metadata);
    }
}