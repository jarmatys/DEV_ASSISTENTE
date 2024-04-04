namespace ASSISTENTE.Contract.Internal.Knowledge.Queries.Answer;

public sealed class AnswerResponse
{
    private AnswerResponse(string text)
    {
        Text = text;
    }
    
    public string Text { get; }
    
    public static AnswerResponse Create(string text)
    {
        return new AnswerResponse(text);
    }
}