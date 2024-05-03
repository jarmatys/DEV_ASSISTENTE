using System.ComponentModel.DataAnnotations;
using ASSISTENTE.Contract.Requests.Internal.Common.RequestBases;
using ASSISTENTE.Language.Identifiers;

namespace ASSISTENTE.Contract.Requests.Internal.Knowledge.Queries.GetResource;

public sealed class GetResourceRequest : GetRequestBase
{
    [Required] public required Guid ResourceId { get; set; } // TODO: Implement endpoint binder to use strongy typed identifiers
    
    public static GetResourceRequest Create(ResourceId resourceId)
    {
        return new GetResourceRequest
        {
            ResourceId = resourceId
        };
    }
}