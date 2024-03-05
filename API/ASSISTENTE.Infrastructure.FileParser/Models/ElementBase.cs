namespace ASSISTENTE.Infrastructure.FileParser.Models;

public abstract record ElementBase(string Content)
{
    public string Content { get; } = Content;
}