using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace ASSISTENTE.Infrastructure.FileParser.Extensions;

public static class MarkDownExtensions
{
    public static string GetContent(this LeafBlock leaf)
    {
        if (leaf.Inline == null) return string.Empty;

        var contentsList = leaf.Inline
            .Descendants()
            .OfType<LiteralInline>()
            .Select(x => x.Content)
            .ToList();

        return string.Join(" ", contentsList);
    }
    
}