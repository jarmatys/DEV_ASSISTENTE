using ASSISTENTE.Common.Results;

namespace ASSISTENTE.Domain.Entities.Questions.Errors;

public static class QuestionErrors
{
    public static readonly Error ContextNotProvided = new(
        "Question.ContextNotProvided", "Context not provided.");
    
    public static readonly Error AnswerNotExist = new(
        "Question.AnswerNotCreated", "Answer for this question not exist.");
}