namespace ASSISTENTE.Language.Enums;

public enum QuestionProgress
{
    Init = 1,
    
    ResolvingContext = 2,
    ContextResolved = 3,
    
    SearchingForFiles = 4,
    FilesFound = 5,
    
    CreatingEmbeddings = 6,
    EmbeddingsCreated = 7,
    
    SearchingForResources = 8,
    ResourcesFound = 9,
    
    Answering = 10,
    Answered = 11,
    
    Ready = 12
}