using SOFTURE.Results;

namespace ASSISTENTE.Domain.Entities.Questions.Errors;

public static class QuestionStateErrors
{
    public static readonly Error UnableToSetContext = new(
        "Question.State.UnableToSetContext", "Unable to set context.");
    
    public static readonly Error UnableToGenerateAnswer = new(
        "Question.State.UnableToGenerateAnswer", "Unable to generate answer.");
}