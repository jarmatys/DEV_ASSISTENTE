using CSharpFunctionalExtensions;
using Neo4j.Driver;

namespace ASSISTENTE.Infrastructure.Neo4J.Contracts;

public interface INeo4JService
{
    Task<Result> CreateGraph(string database, string creationQuery);
    Task<Result<IReadOnlyList<IRecord>>> Search(string database, string query, object? queryParams = null);
}