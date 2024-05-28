namespace ASSISTENTE.Common.Correlation.ValueObjects;

public sealed record CorrelationId(Guid Value)
{
    public static implicit operator CorrelationId(Guid value) => new(value);
    public static implicit operator Guid(CorrelationId id) => id.Value;
    public static implicit operator string(CorrelationId id) => id.Value.ToString();
    
    public static CorrelationId Parse(string value) => new(Guid.Parse(value));
}