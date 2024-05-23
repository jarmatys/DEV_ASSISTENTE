using CommandLine;

namespace ASSISTENTE.Playground.Commons;

internal sealed class PlaygroundParams
{
    [Option('q', "question", Required = false, HelpText = "Question to ask the playground")]
    public string? Question { get; set; }
    
    [Option('l', "learn", Required = false, HelpText = "Learn from a files")]
    public bool Learn { get; set; }
    public bool IsValid => Learn || !string.IsNullOrWhiteSpace(Question);
    public static string NotValidMessage => "Select an option: -l or -q";
}