using ASSISTENTE.Common.Results;

namespace ASSISTENTE.Infrastructure.FileParser.Errors;

internal static class FileParserErrors
{
    public static readonly Error UnsupportedBlock = new(
        "FileParser.UnsupportedBlock", "Block is not supported");
    
    public static readonly Error EmptyContent = new(
        "FileContent.EmptyContent", "File content is empty");
}