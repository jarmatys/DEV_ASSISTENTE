namespace ASSISTENTE.Language.Common;

public abstract record IdentifierBase<T>(T Value) : IIdentifier;