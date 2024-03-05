namespace ASSISTENTE.Infrastructure.FileParser.Models;

public sealed record Heading(string Content, int Level) : ElementBase(Content)
{
    public int Level { get; } = Level;
}