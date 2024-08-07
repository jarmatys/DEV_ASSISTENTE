using ASSISTENTE.Domain.Common.Interfaces;
using SOFTURE.Results;

namespace ASSISTENTE.Application.Abstractions;

public static class RepositoryErrors<TEntity>
    where TEntity : IEntity
{
    public static readonly Error NotFound = new($"{typeof(TEntity).Name}Repository.NotFound", $"{typeof(TEntity).Name} not found");
}