using ASSISTENTE.Domain.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ASSISTENTE.Persistence.MSSQL.Converters;

internal sealed class ResourceTypeConverter()
    : ValueConverter<ResourceType, string>(
        v => v.ToString(),
        v => (ResourceType)Enum.Parse(typeof(ResourceType), v)
    );