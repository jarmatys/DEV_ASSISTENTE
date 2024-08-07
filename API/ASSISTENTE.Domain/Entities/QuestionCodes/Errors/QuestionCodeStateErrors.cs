using SOFTURE.Results;

namespace ASSISTENTE.Domain.Entities.QuestionCodes.Errors;

public static class QuestionCodeStateErrors
{
    public static readonly Error UnableToCreateEmbeddings = new(
        "QuestionCode.State.UnableToCreateEmbeddings", "Unable to create embeddings.");
    
    public static readonly Error UnableToAddFiles = new(
        "QuestionCode.State.UnableToAddFiles", "Unable to find add for the question code.");
    
    public static readonly Error UnableToAddResources = new(
        "QuestionCode.State.UnableToAddResources", "Unable to add resources for the question code.");
    
    public static readonly Error UnableToComplete = new(
        "QuestionCode.State.UnableToComplete", "Unable to complete the question code.");
}