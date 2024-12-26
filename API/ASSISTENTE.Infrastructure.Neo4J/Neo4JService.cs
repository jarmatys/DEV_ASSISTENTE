using ASSISTENTE.Infrastructure.Neo4J.Contracts;
using CSharpFunctionalExtensions;
using Neo4j.Driver;

namespace ASSISTENTE.Infrastructure.Neo4J;

internal sealed class Neo4JService(IDriver driver) : INeo4JService
{
    public async Task<Result> CreateGraph(string database, string creationQuery)
    {
        const string exampleCreationQuery = """
                    UNWIND [
                        {id: '1', username: 'Adrian', access_level: 'user', is_active: '1', lastlog: '2023-06-12'},
                        {id: '5', username: 'Barbara', access_level: 'admin', is_active: '1', lastlog: '2023-06-11'}
                    ] AS user
                    MERGE (u:User {id: user.id})
                    SET u.username = user.username,
                        u.access_level = user.access_level,
                        u.is_active = user.is_active,
                        u.lastlog = user.lastlog;

                    UNWIND [
                        {user1_id: '1', user2_id: '5'}
                    ] AS connection
                    MATCH (u1:User {id: connection.user1_id})
                    MATCH (u2:User {id: connection.user2_id})
                    MERGE (u1)-[:CONNECTED_TO]->(u2);
                    """;
        
        // var queryConfig = new QueryConfig(database: database);

        await using var session = driver.AsyncSession();

        await session.ExecuteWriteAsync(async tx =>
        {
            // foreach (var user in users)
            // {
            //     await tx.RunAsync(
            //         "MERGE (u:User {id: $id}) " +
            //         "SET u.username = $username, u.access_level = $accessLevel, u.is_active = $isActive, u.lastlog = $lastlog",
            //         new
            //         {
            //             id = user.Id,
            //             username = user.Username,
            //             accessLevel = user.AccessLevel,
            //             isActive = user.IsActive,
            //             lastlog = user.Lastlog
            //         });
            // }

            // await session.ExecuteWriteAsync(async tx =>
            // {
            //     foreach (var connection in connections)
            //     {
            //         await tx.RunAsync(
            //             "MATCH (u1:User {id: $user1Id}), (u2:User {id: $user2Id}) " +
            //             "MERGE (u1)-[:CONNECTED_TO]->(u2)",
            //             new
            //             {
            //                 user1Id = connection.User1Id,
            //                 user2Id = connection.User2Id
            //             });
            //     }
            // });
        });

        await driver.DisposeAsync();

        return Result.Success();
    }

    public async Task<Result<IReadOnlyList<IRecord>>> Search(string database, string query,
        object? queryParams = null)
    {
        // https://github.com/neo4j-examples/movies-dotnetcore-bolt/blob/main/Repositories/MovieRepository.cs

        var queryConfig = new QueryConfig(database: database);

        var executableQuery = driver
            .ExecutableQuery(query)
            .WithConfig(queryConfig);

        if (queryParams != null)
            executableQuery = executableQuery.WithParameters(queryParams);

        var (queryResults, _) = await executableQuery.ExecuteAsync();

        return Result.Success(queryResults);
    }
}