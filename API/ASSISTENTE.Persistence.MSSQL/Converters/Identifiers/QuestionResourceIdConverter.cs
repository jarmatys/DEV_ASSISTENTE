using ASSISTENTE.Language.Identifiers;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ASSISTENTE.Persistence.MSSQL.Converters.Identifiers;

internal sealed class QuestionResourceIdConverter()
    : ValueConverter<QuestionResourceId, int>(
        v => v.Value,
        v => new QuestionResourceId(v)
    );