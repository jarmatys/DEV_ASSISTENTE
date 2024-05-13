using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ASSISTENTE.Persistence.Configuration.Converters;

internal sealed class DateTimeConverter()
    : ValueConverter<DateTime, DateTime>(
        v => v.ToUniversalTime(),
        v => v);