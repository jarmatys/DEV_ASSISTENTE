namespace ASSISTENTE.Language.Identifiers;

public sealed record QuestionId(Guid Value) : IdentifierBase<Guid>(Value)
{
    public static implicit operator QuestionId(Guid value) => new(value);
    public static implicit operator Guid(QuestionId questionId) => questionId.Value;
    
    public override string ToString() => Value.ToString();
}
