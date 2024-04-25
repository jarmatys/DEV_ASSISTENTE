namespace ASSISTENTE.Language.Identifiers;

public sealed record AnswerId(int Value) : IIdentifier
{
    public static implicit operator AnswerId(int value) => new(value);
    public static implicit operator int(AnswerId answerId) => answerId.Value;
}

    
   
