using ASSISTENTE.Contract.Requests.Internal.Common.RequestBases;

namespace ASSISTENTE.Contract.Requests.Internal.Common;

public abstract class PaginationRequestBase : GetRequestBase
{
    public int Page { get; set; } = 1;
    public int Elements { get; set; } = 10;
}