namespace ASSISTENTE.Common;

public record Error(string Type, string Description)
{
    public static readonly Error None = new(string.Empty, string.Empty);
}