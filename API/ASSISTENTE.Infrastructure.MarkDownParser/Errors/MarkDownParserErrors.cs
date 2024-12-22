using SOFTURE.Results;

namespace ASSISTENTE.Infrastructure.MarkDownParser.Errors;

internal static class MarkDownParserErrors
{
    public static readonly Error UnsupportedBlock = new(
        "MarkDownParser.UnsupportedBlock", "Block is not supported");
    
    public static readonly Error EmptyContent = new(
        "MarkDownParser.EmptyContent", "File content is empty");
    
    public static readonly Error OnlyHeadersNotAllowed = new(
        "MarkDownParser.OnlyHeadersNotAllowed", "Notes with only headers are not allowed");
    
    public static readonly Error UnsupportedMedia = new(
        "MarkDownParser.UnsupportedMedia", "Media type is not supported");
}