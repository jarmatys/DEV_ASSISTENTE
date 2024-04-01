namespace ASSISTENTE.Infrastructure.LLM.ValueObjects;

public sealed class LLMClient
{
    private LLMClient(string name)
    {
        Name = name;
    }
    
    public string Name { get; }
    
    public static LLMClient Create(string name)
    {
        return new LLMClient(name);
    }
}