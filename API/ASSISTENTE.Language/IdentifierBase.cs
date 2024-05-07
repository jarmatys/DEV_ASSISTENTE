namespace ASSISTENTE.Language;

public abstract record IdentifierBase<T>(T Value) : IIdentifier;