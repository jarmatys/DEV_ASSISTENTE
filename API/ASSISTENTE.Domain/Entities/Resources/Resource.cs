using ASSISTENTE.Domain.Commons;
using ASSISTENTE.Domain.Entities.Questions;
using ASSISTENTE.Domain.Entities.Resources.Enums;

namespace ASSISTENTE.Domain.Entities.Resources;

public sealed class Resource : AuditableEntity<Guid>
{
    private Resource()
    {
    }
    
    private Resource(string content, string title, ResourceType type, IEnumerable<float> embeddings)
    {
        Id = Guid.NewGuid();
        
        Content = content;
        Title = title;
        Type = type;
        Embeddings = embeddings;
        
        Questions = new List<QuestionResource>();
    }
    
    public string Content { get; private set; } = null!;
    public string Title { get; private set; } = null!;
    public ResourceType Type { get; private set; }
    public IEnumerable<float> Embeddings { get; private set; } = null!;

    #region NAVIGATION PROPERTIES

    public ICollection<QuestionResource> Questions { get; private set; } = null!;

    #endregion
    
    public static Result<Resource> Create(string content, string title, ResourceType type, IEnumerable<float> embeddings)
    {
        return new Resource(content, title, type, embeddings);
    }
}