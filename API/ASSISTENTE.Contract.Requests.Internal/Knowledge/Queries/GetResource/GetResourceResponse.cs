using ASSISTENTE.Contract.Requests.Internal.Knowledge.Queries.GetResource.Models;
using ASSISTENTE.Language.Enums;

namespace ASSISTENTE.Contract.Requests.Internal.Knowledge.Queries.GetResource;

public sealed record GetResourceResponse(
    string Title,
    string Content,
    ResourceType Type,
    List<QuestionDto> Questions
);