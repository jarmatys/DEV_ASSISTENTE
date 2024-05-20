using ASSISTENTE.Language.Identifiers;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ASSISTENTE.Persistence.Configuration.Converters.Identifiers;

internal sealed class QuestionFileIdConverter()
    : ValueConverter<QuestionFileId, int>(
        v => v.Value,
        v => new QuestionFileId(v)
    );