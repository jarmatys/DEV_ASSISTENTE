using ASSISTENTE.Contract.Requests.Internal.Common;

namespace ASSISTENTE.Contract.Requests.Internal.Knowledge.Queries.GetResources;

public sealed class GetResourcesRequest : PaginationRequestBase
{
    public static GetResourcesRequest Create(int page, int elements)
    {
        return new GetResourcesRequest
        {
            Page = page,
            Elements = elements
        };
    }
}