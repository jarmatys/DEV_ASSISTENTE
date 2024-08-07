using CSharpFunctionalExtensions;
using SOFTURE.Results;

namespace ASSISTENTE.Infrastructure.LLM.Contracts;

public sealed class Answer : ValueObject
{
    private Answer(string text, string context, LlmClient client, Audit audit)
    {
        Text = text;
        Context = context;
        Client = client;
        Audit = audit;
    }

    public string Text { get; }
    public LlmClient Client { get; }
    public Audit Audit { get; }
    
    private string Context { get; }
    
    public static Result<Answer> Create(
        string? text, 
        string context,
        LlmClient client, 
        Audit audit)
    {
        if (string.IsNullOrEmpty(text))
            return Result.Failure<Answer>(CommonErrors.EmptyParameter.Build());
        
        return new Answer(text, context, client, audit);
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Text;
        yield return Context;
        yield return Client;
        yield return Audit;
    }
}