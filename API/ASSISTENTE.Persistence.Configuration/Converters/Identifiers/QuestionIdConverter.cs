using ASSISTENTE.Language.Identifiers;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ASSISTENTE.Persistence.Configuration.Converters.Identifiers;

internal sealed class QuestionIdConverter()
    : ValueConverter<QuestionId, Guid>(
        v => v.Value,
        v => new QuestionId(v)
    );