namespace ASSISTENTE.Infrastructure.MarkDownParser.Models;

internal sealed record Paragraph(string Content) : ElementBase(Content)
{
}