using ASSISTENTE.Persistence.MSSQL.Converters;
using Microsoft.EntityFrameworkCore;

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