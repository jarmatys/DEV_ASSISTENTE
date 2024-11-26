using CommandLine;

namespace ASSISTENTE.Playground.Common;

internal sealed class PlaygroundParams
{
    [Option('q', "question", Required = false, HelpText = "Question to ask the playground")]
    public string? Question { get; set; }
    
    [Option('l', "learn", Required = false, HelpText = "Learn from a files")]
    public bool Learn { get; set; }
    
    [Option('r', "run", Required = false, HelpText = "Start the playground")]
    public bool Run { get; set; }
    
    public bool IsValid => Learn || !string.IsNullOrWhiteSpace(Question) || Run;
    public static string NotValidMessage => "Select an option: -l, -q or -r";
}