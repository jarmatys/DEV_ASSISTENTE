using ASSISTENTE.Domain.Commons;
using ASSISTENTE.Domain.Entities.Answers;
using ASSISTENTE.Domain.Entities.Questions.Enums;
using ASSISTENTE.Domain.Entities.Resources;

namespace ASSISTENTE.Domain.Entities.Questions;

public sealed class Question : AuditableEntity
{
    private Question(string text, QuestionContext context)
    {
        Text = text;
        Context = context;
        
        Resources = new List<QuestionResource>();
        Embeddings = new List<float>();
    }
    
    public string Text { get; private set; }
    public QuestionContext Context { get; private set; }
    public IEnumerable<float> Embeddings { get; private set; }
    
    # region NAVIGATION PROPERTIES
    
    public Answer Answer { get; private set; } = null!;
    public ICollection<QuestionResource> Resources { get; private set; }
    
    # endregion
    
    public static Result<Question> Create(string text, QuestionContext context)
    {
        return new Question(text, context);
    }
    
    public Result AddResource(IEnumerable<Resource> resources)
    {
        var questionResources = resources
            .Select(r => QuestionResource.Create(r.Id));

        foreach (var resource in questionResources)
        {
            if (resource.IsFailure)
                return Result.Failure(resource.Error);
            
            Resources.Add(resource.Value);
        }
        
        return Result.Success();
    }
    
    public Result AddEmbeddings(IEnumerable<float> embeddings)
    {
        Embeddings = embeddings.ToList();
        
        return Result.Success();
    }
    
    public string GetContext()
    {
        return Context.ToString();
    }
    
    public Result SetAnswer(Answer answer)
    {
        Answer = answer;
        
        return Result.Success();
    }
}