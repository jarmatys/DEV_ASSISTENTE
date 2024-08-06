using ASSISTENTE.Language.Common;

namespace ASSISTENTE.Language.Identifiers;

public sealed record AnswerId(int Value) : IdentifierBase<int>(Value)
{
    public static implicit operator AnswerId(int value) => new(value);
    public static implicit operator int(AnswerId id) => id.Value;
    
    public override string ToString() => Value.ToString();
}