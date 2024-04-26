using ASSISTENTE.Domain.Commons;
using ASSISTENTE.Domain.Entities.Answers;
using ASSISTENTE.Domain.Entities.Questions.Enums;
using ASSISTENTE.Domain.Entities.Questions.Events;
using ASSISTENTE.Domain.Entities.Resources;
using ASSISTENTE.Language.Identifiers;

namespace ASSISTENTE.Domain.Entities.Questions;

public sealed class Question : AuditableEntity<QuestionId>
{
    private Question(string text, string? connectionId, QuestionContext context)
    {
        Id = new QuestionId(Guid.NewGuid());
        
        Text = text;
        ConnectionId = connectionId;
        Context = context;

        Embeddings = null;
        Resources = new List<QuestionResource>();

        RaiseEvent(new QuestionCreatedEvent(Id, connectionId));
    }

    public string Text { get; private set; }
    public string? ConnectionId { get; private set; }
    public QuestionContext Context { get; private set; }
    public List<float>? Embeddings { get; private set; }

    # region NAVIGATION PROPERTIES

    public Answer Answer { get; private set; } = null!;
    public ICollection<QuestionResource> Resources { get; private set; } 

    # endregion

    public static Result<Question> Create(string text, string? connectionId, QuestionContext context)
    {
        return new Question(text, connectionId, context);
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
        
        RaiseEvent(new ResourcesAttachedEvent(Id));

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