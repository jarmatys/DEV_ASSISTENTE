using ASSISTENTE.Common.Results;

namespace ASSISTENTE.Infrastructure.Errors;

internal static class FileParserErrors
{
    public static readonly Error UnsupportedFormat = new(
        "FileParser.UnsupportedFormat", "File format is not supported");
}