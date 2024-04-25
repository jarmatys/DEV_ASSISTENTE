using ASSISTENTE.Persistence.MSSQL.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ASSISTENTE.Persistence.MSSQL.Extensions;

internal static class IdentifiersExtensions
{
    public static void ConfigureStrongyTypeIdentifiers(this ModelBuilder modelBuilder)
    {
        var entities = modelBuilder.Model.GetEntityTypes();
        var primaryKeys = entities.Select(x => x.FindPrimaryKey()).ToList();

        foreach (var primaryKey in primaryKeys)
        {
            if (primaryKey == null)
                throw new ConfiguratonException("Primary key (PK) for new entity not found");
            
            primaryKey.Properties[0].ValueGenerated = ValueGenerated.OnAdd;
        }
    }
}