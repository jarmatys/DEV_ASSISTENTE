namespace ASSISTENTE.Infrastructure.MarkDownParser.Models;

internal sealed record Heading(string Content, int Level) : ElementBase(Content)
{
    public int Level { get; } = Level;
}