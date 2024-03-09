namespace ASSISTENTE.Common.Results;

public record Error(string Type, string Description)
{
    public static readonly Error None = new(string.Empty, string.Empty);
    
    public static Error Create(string type, string description) => new(type, description);
    
    public override string ToString() => $"{Type}: {Description}";
    
    public string Build() => $"{Type}: {Description}";
}