using ASSISTENTE.Infrastructure.MarkDownParser.Contracts.Models;

namespace ASSISTENTE.Infrastructure.MarkDownParser.Models;

internal sealed record Paragraph(string Content, List<string> Urls) : ElementBase(Content)
{
    public List<string> Urls { get; } = Urls;
    
    public override string GetContent()
    {
        var content = $"Paragraph content: {Content}";
        
        if (Urls.Count != 0)
            content += $" | Urls connected with this paragraph: {string.Join(" ", Urls)}";

        return content;
    }
}