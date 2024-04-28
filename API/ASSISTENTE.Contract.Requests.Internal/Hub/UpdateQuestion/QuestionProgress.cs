namespace ASSISTENTE.Contract.Requests.Internal.Hub.UpdateQuestion;

public enum QuestionProgress
{
    Started = 1,
    ResourcesFound = 2,
    ResolvingContext = 3,
    ContextResolved = 4,
    Answering = 5,
    Answered = 6
}