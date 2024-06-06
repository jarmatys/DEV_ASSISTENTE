using ASSISTENTE.Domain.Common;
using ASSISTENTE.Domain.Common.Interfaces;
using ASSISTENTE.Domain.Entities.Answers;
using ASSISTENTE.Domain.Entities.Answers.ValueObjects;
using ASSISTENTE.Domain.Entities.QuestionCodes;
using ASSISTENTE.Domain.Entities.QuestionNotes;
using ASSISTENTE.Domain.Entities.Questions.Errors;
using ASSISTENTE.Domain.Entities.Questions.Events;
using ASSISTENTE.Domain.Entities.Resources;
using ASSISTENTE.Language.Enums;
using ASSISTENTE.Language.Identifiers;

namespace ASSISTENTE.Domain.Entities.Questions;

public sealed class Question : AuditableEntity<QuestionId>, IAggregateRoot
{
    // TODO: Added STATE property + machine state to handle transitions between states

    private Question()
    {
        Resources = new List<QuestionResource>();
        Files = new List<QuestionFile>();
    }

    private Question(string text, string? connectionId)
    {
        Id = new QuestionId(Guid.NewGuid());

        Text = text;
        ConnectionId = connectionId;
        Context = null;

        Resources = new List<QuestionResource>();
        Files = new List<QuestionFile>();

        RaiseEvent(new QuestionCreatedEvent(Id));
    }

    public string Text { get; private set; } = null!;
    public string? ConnectionId { get; private set; }
    public QuestionContext? Context { get; private set; }
    
    # region NAVIGATION PROPERTIES

    public Answer? Answer { get; private set; }
    public QuestionCode? CodeContext { get; private set; }
    public QuestionNote? NoteContext { get; private set; }

    public ICollection<QuestionResource> Resources { get; private set; }
    public ICollection<QuestionFile> Files { get; private set; }

    # endregion

    public static Result<Question> Create(string text, string? connectionId)
    {
        return new Question(text, connectionId);
    }

    public Result AddResources(IEnumerable<Resource> resources)
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

    public Result AddFiles(string fileText)
    {
        return QuestionFile.Create(fileText)
            .Tap(questionFile => Files.Add(questionFile))
            .Tap(_ => RaiseEvent(new FilesAttachedEvent(Id)));
    }

    public Result AddEmbeddings(IEnumerable<float> embeddings)
    {
        RaiseEvent(new EmbeddingsCreatedEvent(Id));

        return Context switch
        {
            QuestionContext.Error => Result.Failure(QuestionErrors.WrongContext.Build()),
            QuestionContext.Code => CodeContext!.AddEmbeddings(embeddings),
            QuestionContext.Note => NoteContext!.AddEmbeddings(embeddings),
            _ => Result.Failure(QuestionErrors.ContextNotProvided.Build())
        };
    }

    public Result AddContext(QuestionContext context)
    {
        if (context == QuestionContext.Error)
            return Result.Failure(QuestionErrors.WrongContext.Build());

        Context = context;

        return context switch
        {
            QuestionContext.Code => QuestionCode.Create(Id).Tap(questionCode => CodeContext = questionCode),
            QuestionContext.Note => QuestionNote.Create(Id).Tap(questionNote => NoteContext = questionNote),
            _ => Result.Failure(QuestionErrors.ContextNotProvided.Build())
        };
    }

    public Result AddAnswer(string text, string prompt, LlmMetadata metadata)
    {
        return Answer.Create(text, prompt, metadata)
            .Tap(answer => Answer = answer)
            .Tap(_ => RaiseEvent(new AnswerAttachedEvent(Id)));
    }

    public Result<string> GetContext()
    {
        return Context is not null
            ? Context.ToString()!
            : Result.Failure<string>(QuestionErrors.ContextNotProvided.Build());
    }

    public Result<List<float>> GetEmbeddings()
    {
        return Context switch
        {
            QuestionContext.Error => Result.Failure<List<float>>(QuestionErrors.WrongContext.Build()),
            QuestionContext.Code => CodeContext!.GetEmbeddings(),
            QuestionContext.Note => NoteContext!.GetEmbeddings(),
            _ => Result.Failure<List<float>>(QuestionErrors.ContextNotProvided.Build())
        };
    }

    public Result<string> GetAnswer()
    {
        return Answer?.Text ?? Result.Failure<string>(QuestionErrors.AnswerNotExist.Build());
    }

    public Result<string> BuildEmbeddableText()
    {
        var fileNames = Files.Select(x => x.Text);

        return Context switch
        {
            QuestionContext.Note => Text,
            QuestionContext.Code => $"{Text} | {string.Join(", ", fileNames)}",
            QuestionContext.Error => Result.Failure<string>(QuestionErrors.WrongContext.Build()),
            _ => Result.Failure<string>(QuestionErrors.ContextNotProvided.Build())
        };
    }

    public Result<string> BuildContext()
    {
        var resourcesContent = Resources.Select(x => x.Resource).Select(x => x.Content);

        return string.Join(Environment.NewLine, resourcesContent);
    }
}