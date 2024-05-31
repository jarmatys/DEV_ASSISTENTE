using ASSISTENTE.Application.Abstractions.Interfaces;
using ASSISTENTE.Infrastructure.CodeParser;
using ASSISTENTE.Infrastructure.CodeParser.Contracts;
using ASSISTENTE.Infrastructure.MarkDownParser;
using ASSISTENTE.Infrastructure.MarkDownParser.Contracts;
using Microsoft.Extensions.Logging;

namespace ASSISTENTE.Infrastructure.Services.Parsers;

public sealed partial class FileParser(
    IMarkDownParser markDownParser,
    ICodeParser codeParser,
    ILogger<FileParser> logger) : IFileParser
{
    private static readonly IEnumerable<string> PathsToIgnore =
    [
        ".obsidian",
        ".git",
        ".vscode",
        ".github",
        "bin",
        "obj",
        ".idea"
    ];

    private static List<string> GetPaths(string rootDirectory)
    {
        var paths = new List<string>();

        GetFilesRecursively(rootDirectory, paths);

        return paths;
    }

    private static void GetFilesRecursively(string directory, ICollection<string> paths)
    {
        foreach (var file in Directory.GetFiles(directory))
        {
            paths.Add(file);
        }

        foreach (var subdirectory in Directory.GetDirectories(directory))
        {
            if (PathsToIgnore.Any(subdirectory.Contains))
                continue;

            GetFilesRecursively(subdirectory, paths);
        }
    }
}