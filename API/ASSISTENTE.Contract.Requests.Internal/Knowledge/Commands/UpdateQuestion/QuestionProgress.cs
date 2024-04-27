namespace ASSISTENTE.Contract.Requests.Internal.Knowledge.Commands.UpdateQuestion;

public enum QuestionProgress
{
    Started = 1,
    ResourcesFound = 2,
    BuildingContext = 3,
    Answering = 4,
    Answered = 5
}