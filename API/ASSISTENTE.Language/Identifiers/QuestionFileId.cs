using SOFTURE.Language.Common;

namespace ASSISTENTE.Language.Identifiers;

public sealed record QuestionFileId(int Value) : IdentifierBase<int>(Value)
{
    public static implicit operator QuestionFileId(int value) => new(value);
    public static implicit operator int(QuestionFileId id) => id.Value;
    
    public override string ToString() => Value.ToString();
}