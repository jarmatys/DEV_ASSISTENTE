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
        var contextActionResult = ExecuteContextAction(
            onCode: () => CodeContext!.AddResources(),
            onNote: () => NoteContext!.AddResources()
        );

        return contextActionResult
            .Bind(() =>
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
            })
            .Tap(() => RaiseEvent(new ResourcesAttachedEvent(Id)));
    }

    public Result AddFiles(string fileText)
    {
        var contextActionResult = ExecuteContextAction(
            onCode: () => CodeContext!.AddFiles()
        );

        return contextActionResult
            .Bind(() => QuestionFile.Create(fileText))
            .Tap(questionFile => Files.Add(questionFile))
            .Tap(_ => RaiseEvent(new FilesAttachedEvent(Id)));
    }

    public Result AddEmbeddings(IEnumerable<float> embeddings)
    {
        var contextActionResult = ExecuteContextAction(
            onCode: () => CodeContext!.AddEmbeddings(embeddings),
            onNote: () => NoteContext!.AddEmbeddings(embeddings)
        );

        return contextActionResult
            .Tap(() => RaiseEvent(new EmbeddingsCreatedEvent(Id)));
    }

    public Result ResolveContext(QuestionContext context)
    {
        return PerformIfPossible(QuestionActions.ResolveContext, QuestionStateErrors.UnableToSetContext)
            .Tap(() => Context = context)
            .Bind(() =>
            {
                if (context == QuestionContext.Error)
                    return Result.Failure(QuestionErrors.WrongContext.Build());

                return context switch
                {
                    QuestionContext.Code => QuestionCode.Create(Id).Tap(questionCode => CodeContext = questionCode),
                    QuestionContext.Note => QuestionNote.Create(Id).Tap(questionNote => NoteContext = questionNote),
                    _ => Result.Failure(QuestionErrors.ContextNotProvided.Build())
                };
            });
    }

    public Result AddAnswer(string text, string prompt, LlmMetadata metadata)
    {
        var contextActionResult = ExecuteContextAction(
            onCode: () => CodeContext!.Complete(),
            onNote: () => NoteContext!.Complete()
        );

        return contextActionResult
            .Tap(() => PerformIfPossible(QuestionActions.GenerateAnswer, QuestionStateErrors.UnableToGenerateAnswer))
            .Bind(() => Answer.Create(text, prompt, metadata))
            .Tap(answer => Answer = answer)
            .Tap(_ => RaiseEvent(new AnswerAttachedEvent(Id)));
    }

    private Result ExecuteContextAction(
        Func<Result>? onCode = null,
        Func<Result>? onNote = null)
    {
        return Context switch
        {
            QuestionContext.Error => Result.Failure(QuestionErrors.WrongContext.Build()),
            QuestionContext.Code => onCode?.Invoke() ?? ErrorResult(nameof(onCode)),
            QuestionContext.Note => onNote?.Invoke() ?? ErrorResult(nameof(onNote)),
            _ => Result.Failure(QuestionErrors.ContextNotProvided.Build())
        };

        Result ErrorResult(string action) =>
            Result.Failure(QuestionErrors.OperationNotSupported.Build($"Context: {Context} - Action: {action}"));
    }
}