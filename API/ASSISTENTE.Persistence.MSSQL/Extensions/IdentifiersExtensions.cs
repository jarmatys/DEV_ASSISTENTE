using ASSISTENTE.Language;
using ASSISTENTE.Persistence.MSSQL.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ASSISTENTE.Persistence.MSSQL.Extensions;

internal static class IdentifiersExtensions
{
    public static void ConfigureStrongyIdentifiers(this ModelBuilder modelBuilder)
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
    
    public static void ConfigureIdentifier<TIdentifier, TConverter, TType>(this ModelConfigurationBuilder configurationBuilder)
        where TIdentifier : IIdentifier
        where TConverter : ValueConverter<TIdentifier, TType>
    {
        configurationBuilder
            .Properties<TIdentifier>()
            .HaveConversion<TConverter>();
    }
}