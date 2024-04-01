using ASSISTENTE.Common.Errors;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Infrastructure.LLM.ValueObjects;

public sealed class Answer
{
    private Answer(string text, string context, LLMClient client, Audit audit)
    {
        Text = text;
        Context = context;
        Client = client;
        Audit = audit;
    }

    public string Text { get; }
    public string Context { get; }
    public LLMClient Client { get; }
    public Audit Audit { get; }
    
    public static Result<Answer> Create(
        string? text, 
        string context,
        LLMClient client, 
        Audit audit)
    {
        if (string.IsNullOrEmpty(text))
            return Result.Failure<Answer>(CommonErrors.EmptyParameter.Build());
        
        return new Answer(text, context, client, audit);
    }
}