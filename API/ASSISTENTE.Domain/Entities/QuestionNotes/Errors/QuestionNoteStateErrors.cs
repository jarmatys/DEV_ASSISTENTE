using SOFTURE.Results;

namespace ASSISTENTE.Domain.Entities.QuestionNotes.Errors;

public static class QuestionNoteStateErrors
{
    public static readonly Error UnableToCreateEmbeddings = new(
        "QuestionNote.State.UnableToCreateEmbeddings", "Unable to create embeddings.");
    
    public static readonly Error UnableToAddResources = new(
        "QuestionNote.State.UnableToAddResources", "Unable to add resources for the question code.");
    
    public static readonly Error UnableToComplete = new(
        "QuestionNote.State.UnableToComplete", "Unable to complete the question code.");
}