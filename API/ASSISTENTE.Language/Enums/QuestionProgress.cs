namespace ASSISTENTE.Language.Enums;

public enum QuestionProgress
{
    Init = 1,
    ResolvingContext = 2,
    ContextResolved = 3,
    SearchingForResources = 4,
    ResourcesFound = 5,
    Answering = 6,
    Answered = 7,
    Ready = 8
}