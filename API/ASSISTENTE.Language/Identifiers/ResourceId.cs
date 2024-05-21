namespace ASSISTENTE.Language.Identifiers;

public sealed record ResourceId(Guid Value) : IdentifierBase<Guid>(Value)
{
    public static implicit operator ResourceId(Guid value) => new(value);
    public static implicit operator Guid(ResourceId id) => id.Value;
    
    public override string ToString() => Value.ToString();
}
