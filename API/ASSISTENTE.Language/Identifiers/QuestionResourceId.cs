namespace ASSISTENTE.Language.Identifiers;

public sealed record QuestionResourceId(int Value) : IdentifierBase<int>(Value)
{
    public static implicit operator QuestionResourceId(int value) => new(value);
    public static implicit operator int(QuestionResourceId questionResourceId) => questionResourceId.Value;
    
    public override string ToString() => Value.ToString();
}