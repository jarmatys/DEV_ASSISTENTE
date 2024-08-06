using ASSISTENTE.Language.Common;
using FastEndpoints;

namespace ASSISTENTE.API.Common.Parsers;

public static class IdentifierParsers
{
    public static ParseResult GuidParser<TIdentifier>(object? input)
        where TIdentifier : IIdentifier
    {
        var success = Guid.TryParse(input?.ToString(), out var result);
        
        var identifier = (TIdentifier)Activator.CreateInstance(typeof(TIdentifier), result)!;
        
        return new ParseResult(success, identifier);
    }
    
    public static ParseResult NumberParser<TIdentifier>(object? input)
        where TIdentifier : IIdentifier
    {
        var success = int.TryParse(input?.ToString(), out var result);
        
        var identifier = (TIdentifier)Activator.CreateInstance(typeof(TIdentifier), result)!;
        
        return new ParseResult(success, identifier);
    }
}