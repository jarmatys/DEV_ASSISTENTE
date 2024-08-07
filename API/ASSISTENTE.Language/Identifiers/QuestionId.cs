using SOFTURE.Language.Common;

namespace ASSISTENTE.Language.Identifiers;

public sealed record QuestionId(Guid Value) : IdentifierBase<Guid>(Value)
{
    public static implicit operator QuestionId(Guid value) => new(value);
    public static implicit operator Guid(QuestionId id) => id.Value;
    
    public override string ToString() => Value.ToString();
}
