using ASSISTENTE.Domain.Commons;
using ASSISTENTE.Domain.Entities.Enums;
using ASSISTENTE.Domain.Entities.Resources;

namespace ASSISTENTE.Domain.Entities.Questions;

public sealed class Question : AuditableEntity // TODO: Prepare repo and use them in RecallAsync method
{
    private Question()
    {
        Resources = new List<QuestionResource>();
    }
    
    private Question(string text, IEnumerable<float> embeddings, IEnumerable<QuestionResource> questionResources)
    {
        Text = text;
        Embeddings = embeddings;
        Resources = questionResources.ToList();
    }
    
    public string Text { get; private set; }
    public QuestionContext  Context { get; private set; }
    public IEnumerable<float> Embeddings { get; private set; }
    
    # region NAVIGATION PROPERTIES
    
    public ICollection<QuestionResource> Resources { get; private set; }
    
    # endregion
    
    public static Result<Question> Create(string text, IEnumerable<float> embeddings, IEnumerable<Resource> resources)
    {
        var questionResources = resources
            .Select(r => QuestionResource.Create(r.Id))
            .Select(r => r.Value);
        
        return new Question(text, embeddings, questionResources);
    }
}