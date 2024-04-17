namespace ASSISTENTE.Contract.Internal.Messages.Knowledge;

public sealed class GenerateAnswerMessage(int questionId, string text)
{
    public int QuestionId { get; set; } = questionId;
    public string Text { get; set; } = text;
}