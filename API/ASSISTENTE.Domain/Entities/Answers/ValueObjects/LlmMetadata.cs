namespace ASSISTENTE.Domain.Entities.Answers.ValueObjects;

public sealed class LlmMetadata : ValueObject
{
    private LlmMetadata(string client, string model, int promptTokens, int completionTokens)
    {
        Client = client;
        Model = model;
        PromptTokens = promptTokens;
        CompletionTokens = completionTokens;
    }
    
    public string Client { get; init; } 
    public string Model { get; init; } 
    public int PromptTokens { get; init; }
    public int CompletionTokens { get; init; }

    public static Result<LlmMetadata> Create(string client, string model, int promptTokens, int completionTokens)
    {
        return new LlmMetadata(client, model, promptTokens, completionTokens);
    }
    
    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Client;
        yield return Model;
        yield return PromptTokens;
        yield return CompletionTokens;
    }
}