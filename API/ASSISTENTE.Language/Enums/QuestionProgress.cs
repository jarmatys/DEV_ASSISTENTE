namespace ASSISTENTE.Language.Enums;

public enum QuestionProgress
{
    Init = 1,
    SearchingForResources = 2,
    ResourcesFound = 3,
    ResolvingContext = 4,
    ContextResolved = 5,
    Answering = 6,
    Answered = 7,
    Ready = 8
}