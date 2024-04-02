using ASSISTENTE.Domain.Entities.Questions.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ASSISTENTE.Persistence.MSSQL.Converters;

internal sealed class QuestionContextConverter()
    : ValueConverter<QuestionContext, string>(
        v => v.ToString(),
        v => (QuestionContext)Enum.Parse(typeof(QuestionContext), v)
    );