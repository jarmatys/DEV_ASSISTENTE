using ASSISTENTE.Contract.Common.RequestBases;

namespace ASSISTENTE.Contract.Common;

public abstract class PaginationRequestBase : GetRequestBase
{
    public int Page { get; set; } = 1;
    public int Elements { get; set; } = 10;
}