
using ASSISTENTE.Contract.Requests.Internal.Knowledge.Queries.GetResources.Models;

namespace ASSISTENTE.Contract.Requests.Internal.Knowledge.Queries.GetResources;

public sealed record GetResourcesResponse(List<ResourceDto> Resources);