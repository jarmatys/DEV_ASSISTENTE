using ASSISTENTE.Common.Results;

namespace ASSISTENTE.Common.Errors;

public static class CommonErrors
{
    public static readonly Error EmptyParameter = new(
        "Common.EmptyParameter", "Parameter cannot be empty.");
}