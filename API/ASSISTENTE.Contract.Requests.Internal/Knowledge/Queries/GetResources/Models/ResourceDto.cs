using ASSISTENTE.Language.Enums;

namespace ASSISTENTE.Contract.Requests.Internal.Knowledge.Queries.GetResources.Models;

public sealed record ResourceDto(Guid ResourceId, string Title, ResourceType Type);