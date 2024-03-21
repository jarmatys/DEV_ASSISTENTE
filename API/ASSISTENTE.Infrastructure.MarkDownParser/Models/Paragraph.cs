namespace ASSISTENTE.Infrastructure.MarkDownParser.Models;

internal sealed record Paragraph(string Content, string Urls) : ElementBase(Content)
{
    public string Urls { get; } = Urls;
    
    public override string GetContent()
    {
        var content = $"Paragraph content: {Content}";
        
        if (!string.IsNullOrEmpty(Urls))
            content += $" | Urls connected with this paragraph: {Urls}";

        return content;
    }
}