using CommandLine;

namespace ASSISTENTE.Playground.Commons;

internal sealed class PlaygroundParams
{
    [Option('r', "reset", Required = false, HelpText = "Reset the playground")]
    public bool Reset { get; set; }

    [Option('q', "question", Required = false, HelpText = "Question to ask the playground")]
    public string? Question { get; set; }
    
    [Option('l', "learn", Required = false, HelpText = "Learn from a file")]
    public bool Learn { get; set; }
}