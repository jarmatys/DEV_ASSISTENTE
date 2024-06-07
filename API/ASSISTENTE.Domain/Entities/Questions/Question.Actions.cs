using ASSISTENTE.Domain.Entities.Answers;
using ASSISTENTE.Domain.Entities.Answers.ValueObjects;
using ASSISTENTE.Domain.Entities.QuestionCodes;
using ASSISTENTE.Domain.Entities.QuestionNotes;
using ASSISTENTE.Domain.Entities.Questions.Errors;
using ASSISTENTE.Domain.Entities.Questions.Events;
using ASSISTENTE.Domain.Entities.Resources;
using ASSISTENTE.Language.Enums;

namespace ASSISTENTE.Domain.Entities.Questions;

public sealed partial class Question
{
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

    public Result ResolveContext(QuestionContext context)
    {
        if (context == QuestionContext.Error)
            return Result.Failure(QuestionErrors.WrongContext.Build());

        return PerformIfPossible(QuestionActions.ResolveContext, QuestionStateErrors.UnableToSetContext)
            .Tap(() => Context = context)
            .Bind(() => context switch
            {
                QuestionContext.Code => QuestionCode.Create(Id).Tap(questionCode => CodeContext = questionCode),
                QuestionContext.Note => QuestionNote.Create(Id).Tap(questionNote => NoteContext = questionNote),
                _ => Result.Failure(QuestionErrors.ContextNotProvided.Build())
            });
    }

    public Result AddAnswer(string text, string prompt, LlmMetadata metadata)
    {
        return Answer.Create(text, prompt, metadata)
            .Tap(answer => Answer = answer)
            .Tap(_ => RaiseEvent(new AnswerAttachedEvent(Id)));
    }
}
