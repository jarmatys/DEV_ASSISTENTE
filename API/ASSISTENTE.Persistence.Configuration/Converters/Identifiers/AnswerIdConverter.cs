using ASSISTENTE.Language.Identifiers;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ASSISTENTE.Persistence.Configuration.Converters.Identifiers;

internal sealed class AnswerIdConverter()
    : ValueConverter<AnswerId, int>(
        v => v.Value,
        v => new AnswerId(v)
    );