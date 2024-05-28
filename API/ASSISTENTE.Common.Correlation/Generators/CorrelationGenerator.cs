using ASSISTENTE.Common.Correlation.ValueObjects;

namespace ASSISTENTE.Common.Correlation.Generators;

public static class CorrelationGenerator
{
    public static CorrelationId Generate() => new(Guid.NewGuid());
}