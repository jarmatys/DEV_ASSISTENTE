using SOFTURE.Results;

namespace ASSISTENTE.Domain.Entities.Questions.Errors;

public static class QuestionErrors
{
    public static readonly Error ContextNotProvided = new(
        "Question.ContextNotProvided", "Context not provided.");
   
    public static readonly Error EmbeddingsNotCreated = new(
        "Question.EmbeddingsNotCreated", "Embeddings not created.");
    
    public static readonly Error WrongContext = new(
        "Question.WrongContext", "Context shoudn't be 'Error' or null.");
    
    public static readonly Error AnswerNotExist = new(
        "Question.AnswerNotCreated", "Answer for this question not exist.");
    
    public static readonly Error OperationNotSupported = new(
        "Question.OperationNotSupported", "Operation not supported.");
}