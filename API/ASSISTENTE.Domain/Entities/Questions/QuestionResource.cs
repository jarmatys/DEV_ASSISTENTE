using ASSISTENTE.Domain.Commons;
using ASSISTENTE.Domain.Entities.Resources;

namespace ASSISTENTE.Domain.Entities.Questions;

public sealed class QuestionResource : AuditableEntity
{
    private QuestionResource(int resourceId)
    {
        ResourceId = resourceId;
    }
    
    # region NAVIGATION PROPERTIES
    
    public int QuestionId { get; private set; }
    public Question Question { get; private set; } = null!;

    public int ResourceId { get; private set; }
    public Resource Resource { get; private set; } = null!;

    # endregion
    
    public static Result<QuestionResource> Create(int resourceId)
    {
        return new QuestionResource(resourceId);
    }
}