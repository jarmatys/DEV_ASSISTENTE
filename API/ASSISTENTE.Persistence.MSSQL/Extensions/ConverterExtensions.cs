using ASSISTENTE.Language;
using ASSISTENTE.Persistence.MSSQL.Converters;
using ASSISTENTE.Persistence.MSSQL.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ASSISTENTE.Persistence.MSSQL.Extensions;

internal static class ConverterExtensions
{
    public static void ConfigureEnum<TEnum>(this ModelConfigurationBuilder configurationBuilder)
        where TEnum : Enum
    {
        configurationBuilder
            .Properties<TEnum>()
            .HaveConversion<EnumConverter<TEnum>>();
    }
}