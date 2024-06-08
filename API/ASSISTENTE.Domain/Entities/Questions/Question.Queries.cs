using ASSISTENTE.Domain.Entities.Questions.Errors;
using ASSISTENTE.Language.Enums;

namespace ASSISTENTE.Domain.Entities.Questions;

public sealed partial class Question
{
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
