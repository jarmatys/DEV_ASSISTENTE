using ASSISTENTE.Language.Common;

namespace ASSISTENTE.Language.Identifiers;

public sealed record QuestionNoteId(Guid Value) : IdentifierBase<Guid>(Value)
{
    public static implicit operator QuestionNoteId(Guid value) => new(value);
    public static implicit operator Guid(QuestionNoteId id) => id.Value;
    
    public override string ToString() => Value.ToString();
}
