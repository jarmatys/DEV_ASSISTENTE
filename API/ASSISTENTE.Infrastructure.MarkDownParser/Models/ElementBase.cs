namespace ASSISTENTE.Infrastructure.MarkDownParser.Models;

public abstract record ElementBase(string Content)
{
    public string Content { get; } = Content;

    public abstract string GetContent();
}