namespace ASSISTENTE.Language.Identifiers;

public sealed record ResourceId(Guid Value) : IIdentifier
{
    public static implicit operator ResourceId(Guid value) => new(value);
    public static implicit operator Guid(ResourceId resourceId) => resourceId.Value;
}
