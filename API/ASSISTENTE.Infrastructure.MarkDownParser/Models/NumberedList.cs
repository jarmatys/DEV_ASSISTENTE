namespace ASSISTENTE.Infrastructure.MarkDownParser.Models;

internal sealed record NumberedList(string Content) : ElementBase(Content)
{
}