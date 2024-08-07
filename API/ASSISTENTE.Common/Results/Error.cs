namespace ASSISTENTE.Common.Results;

public record Error(string Type, string Description)
{
    public static readonly Error None = new(string.Empty, string.Empty);
    
    public static Error Parse(string message) => new(message.Split(": ")[0], message.Split(": ")[1]);
    

    public override string ToString() => $"{Type}: {Description}";
    
    public string Build() => $"{Type}: {Description}";
    public string Build(string message) => $"{Type}: {Description} - {message}";
}