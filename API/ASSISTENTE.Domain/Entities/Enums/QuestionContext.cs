using ASSISTENTE.Domain.Entities.Resources.Enums;

namespace ASSISTENTE.Domain.Entities.Enums;

public enum QuestionContext // TODO: Error shoud be default value
{
    Error = 1,
    Note = ResourceType.Note,
    Code = ResourceType.Code,
}