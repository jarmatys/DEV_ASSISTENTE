using System.ComponentModel.DataAnnotations;
using ASSISTENTE.Language.Identifiers;
using SOFTURE.Contract.Common.RequestBases;

namespace ASSISTENTE.Contract.Requests.Internal.Knowledge.Queries.GetResource;

public sealed class GetResourceRequest : GetRequestBase
{
    [Required] public required ResourceId ResourceId { get; set; }

    public static GetResourceRequest Create(ResourceId resourceId)
    {
        return new GetResourceRequest
        {
            ResourceId = resourceId
        };
    }
}