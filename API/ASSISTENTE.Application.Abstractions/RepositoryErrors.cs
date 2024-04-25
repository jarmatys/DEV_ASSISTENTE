using ASSISTENTE.Common.Results;

namespace ASSISTENTE.Application.Abstractions;

public static class RepositoryErrors
{
    public static readonly Error NotFound = new("Repository.NotFound", "Not found");
}