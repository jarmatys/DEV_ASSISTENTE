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
    
    public static void ConfigureGuidIdentifier<TIdentifier, TConverter>(this ModelConfigurationBuilder configurationBuilder)
        where TIdentifier : IdentifierBase<Guid>
        where TConverter : ValueConverter<TIdentifier, Guid>
    {
        configurationBuilder
            .Properties<TIdentifier>()
            .HaveConversion<TConverter>();
    }
    
    public static void ConfigureNumberIdentifier<TIdentifier, TConverter>(this ModelConfigurationBuilder configurationBuilder)
        where TIdentifier : IdentifierBase<int>
        where TConverter : ValueConverter<TIdentifier, int>
    {
        configurationBuilder
            .Properties<TIdentifier>()
            .HaveConversion<TConverter>();
    }
}