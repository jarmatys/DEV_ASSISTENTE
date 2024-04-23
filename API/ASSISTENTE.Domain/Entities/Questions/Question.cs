using ASSISTENTE.Domain.Commons;
using ASSISTENTE.Domain.Entities.Answers;
using ASSISTENTE.Domain.Entities.Questions.Enums;
using ASSISTENTE.Domain.Entities.Questions.Events;
using ASSISTENTE.Domain.Entities.Resources;

namespace ASSISTENTE.Domain.Entities.Questions;

public sealed class Question : AuditableEntity
{
    private Question()
    {
    }

    private Question(string text, string? connectionId, QuestionContext context)
    {
        Text = text;
        Context = context;

        Resources = new List<QuestionResource>();
        Embeddings = new List<float>();

        RaiseEvent(new QuestionCreatedEvent(Id, connectionId));
    }

    public string Text { get; private set; } = null!;
    public QuestionContext Context { get; private set; }
    public IEnumerable<float> Embeddings { get; private set; } = null!;

    # region NAVIGATION PROPERTIES

    public Answer Answer { get; private set; } = null!;
    public ICollection<QuestionResource> Resources { get; private set; } = null!;

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