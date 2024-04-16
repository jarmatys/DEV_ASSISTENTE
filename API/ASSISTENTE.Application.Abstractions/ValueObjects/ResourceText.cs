namespace ASSISTENTE.Application.Abstractions.ValueObjects;

public sealed class ResourceText
{
    private ResourceText(string title, string content)
    {
        Title = title;
        Content = content;
    }
    
    public string Title { get; }
    public string Content { get; }
    
    public static ResourceText Create(string title, string content)
    {
        return new ResourceText(title, content);
    }
}