namespace ASSISTENTE.Infrastructure.MarkDownParser.Contracts.Models;

public abstract record ElementBase(string Content)
{
    private const string Nbsp = "\u00a0";

    public string Content { get; } = Content.Replace(Nbsp, "");

    public abstract string GetContent();
}