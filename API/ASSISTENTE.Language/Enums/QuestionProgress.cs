namespace ASSISTENTE.Language.Enums;

public enum QuestionProgress
{
    Init = 1,
    ResolvingContext = 2,
    ContextResolved = 3,
    
    SearchingForFiles = 4,
    FilesFound = 5,
    
    SearchingForResources = 6,
    ResourcesFound = 7,
    
    Answering = 8,
    Answered = 9,
    Ready = 10
}