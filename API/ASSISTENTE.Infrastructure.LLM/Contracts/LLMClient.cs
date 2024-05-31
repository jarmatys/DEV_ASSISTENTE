using CSharpFunctionalExtensions;

namespace ASSISTENTE.Infrastructure.LLM.Contracts;

public sealed class LlmClient : ValueObject
{
    private LlmClient(string name)
    {
        Name = name;
    }
    
    public string Name { get; }
    
    public static LlmClient Create(string name)
    {
        return new LlmClient(name);
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Name;
    }
}