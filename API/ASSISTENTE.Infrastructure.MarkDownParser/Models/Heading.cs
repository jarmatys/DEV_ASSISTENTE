using ASSISTENTE.Infrastructure.MarkDownParser.Contracts.Models;

namespace ASSISTENTE.Infrastructure.MarkDownParser.Models;

internal sealed record Heading(string Content, int Level) : ElementBase(Content)
{
    public int Level { get; } = Level;

    public override string GetContent() => $"Heading level {Level}: {Content}";
}