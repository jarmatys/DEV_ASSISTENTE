using ASSISTENTE.Language.Common;

namespace ASSISTENTE.Language.Identifiers;

public sealed record QuestionCodeId(Guid Value) : IdentifierBase<Guid>(Value)
{
    public static implicit operator QuestionCodeId(Guid value) => new(value);
    public static implicit operator Guid(QuestionCodeId id) => id.Value;
    
    public override string ToString() => Value.ToString();
}
