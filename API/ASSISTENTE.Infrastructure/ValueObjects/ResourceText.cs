namespace ASSISTENTE.Infrastructure.ValueObjects;

public sealed class ResourceText
{
    private ResourceText(string title, string content)
    {
        Title = title;
        Content = content;
    }
    
    public string Title { get; set; }
    public string Content { get; set; }
    
    public static ResourceText Create(string title, string content)
    {
        return new ResourceText(title, content);
    }
}