using System.Reflection;
using ASSISTENTE.Domain.Common.Interfaces;
using ASSISTENTE.Language;
using ASSISTENTE.Persistence.Configuration.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ASSISTENTE.Persistence.Configuration.Extensions;

internal static class IdentifiersExtensions
{
    public static void ConfigureStrongyIdentifiers(this ModelBuilder modelBuilder)
    {
        var entities = modelBuilder.Model.GetEntityTypes();
        var primaryKeys = entities
            .Where(x => typeof(IEntity).IsAssignableFrom(x.ClrType)) 
            .Select(x => x.FindPrimaryKey())
            .ToList();

        foreach (var primaryKey in primaryKeys)
        {
            if (primaryKey == null)
                throw new ConfiguratonException("Primary key (PK) for new entity not found");
            
            primaryKey.Properties[0].ValueGenerated = ValueGenerated.OnAdd;
        }
    }
    
    public static void ConfigureStrongyIdentifiers(this ModelConfigurationBuilder configurationBuilder)
    {
        var identifierTypes = typeof(IIdentifier).Assembly.GetTypes()
            .Where(t => t.IsClass && typeof(IIdentifier).IsAssignableFrom(t) && !t.IsAbstract)
            .ToList();

        foreach (var identifierType in identifierTypes)
        {
            var valueType = identifierType.BaseType?.GetGenericArguments().FirstOrDefault();
            if (valueType == typeof(Guid))
            {
                configurationBuilder.RegisterConverter(identifierType, typeof(Guid));
            }
            else if (valueType == typeof(int))
            {
                configurationBuilder.RegisterConverter(identifierType, typeof(int));
            }
        }
    }

    private static void RegisterConverter(this ModelConfigurationBuilder configurationBuilder, Type identifierType, Type valueType)
    {
        if (valueType == typeof(Guid))
        {
            var method = typeof(IdentifiersExtensions)
                .GetMethod(nameof(ConfigureGuidIdentifier), BindingFlags.Static | BindingFlags.NonPublic)
                ?.MakeGenericMethod(identifierType);
            
            method?.Invoke(null, [configurationBuilder]);
        }
        else if (valueType == typeof(int))
        {
            var method = typeof(IdentifiersExtensions)
                .GetMethod(nameof(ConfigureNumberIdentifier), BindingFlags.Static | BindingFlags.NonPublic)
                ?.MakeGenericMethod(identifierType);
            
            method?.Invoke(null, [configurationBuilder]);
        }
    }

    private class GuidToIdentifierConverter<TIdentifier>() : ValueConverter<TIdentifier, Guid>(id => id.Value,
        value => ((TIdentifier)Activator.CreateInstance(typeof(TIdentifier), value)!)!)
        where TIdentifier : IdentifierBase<Guid>;

    private class IntToIdentifierConverter<TIdentifier>() : ValueConverter<TIdentifier, int>(id => id.Value,
        value => ((TIdentifier)Activator.CreateInstance(typeof(TIdentifier), value)!)!)
        where TIdentifier : IdentifierBase<int>;
    
    
    private static void ConfigureGuidIdentifier<TIdentifier>(ModelConfigurationBuilder configurationBuilder)
        where TIdentifier : IdentifierBase<Guid>
    {
        configurationBuilder
            .Properties<TIdentifier>()
            .HaveConversion<GuidToIdentifierConverter<TIdentifier>>();
    }

    private static void ConfigureNumberIdentifier<TIdentifier>(ModelConfigurationBuilder configurationBuilder)
        where TIdentifier : IdentifierBase<int>
    {
        configurationBuilder
            .Properties<TIdentifier>()
            .HaveConversion<IntToIdentifierConverter<TIdentifier>>();
    }
}