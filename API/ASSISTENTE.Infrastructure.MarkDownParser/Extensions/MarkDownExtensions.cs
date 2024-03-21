using Markdig.Helpers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace ASSISTENTE.Infrastructure.MarkDownParser.Extensions;

internal static class MarkDownExtensions
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

    public static string GetUrls(this LeafBlock leaf)
    {
        if (leaf.Inline == null) return string.Empty;

        var urls = leaf.Inline
            .Descendants()
            .OfType<LinkInline>()
            .Select(x => x.Url)
            .ToList();

        return string.Join(" ", urls);
    }
    
    public static string GetCode(this LeafBlock leaf)
    {
        var contentsList = leaf.Lines
            .OfType<StringLine>()
            .Select(x => x.ToString())
            .ToList();

        return string.Join("\n", contentsList);
    }
    
    public static string GetListContent(this ListBlock block)
    {
        var listContent = block
            .Descendants()
            .OfType<LiteralInline>()
            .Select((item, index) => $"{index + 1}. {item.Content}")
            .ToList();
        
        // TODO: Get urls from numbered list
        var urls = block
            .Descendants()
            .OfType<LinkInline>()
            .Select(x => x.Url)
            .ToList();
        
        return string.Join(",\n", listContent);
    }
}