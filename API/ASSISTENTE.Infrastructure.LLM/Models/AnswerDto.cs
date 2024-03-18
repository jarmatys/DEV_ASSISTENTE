namespace ASSISTENTE.Infrastructure.LLM.Models;

public sealed class AnswerDto
{
    private AnswerDto(string text)
    {
        Text = text;
    }

    public string Text { get; }
    
    public static AnswerDto Create(string text)
    {
        return new AnswerDto(text);
    }
}