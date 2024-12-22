using ASSISTENTE.Infrastructure.PromptGenerator.Contracts;
using ASSISTENTE.Infrastructure.PromptGenerator.Interfaces;

namespace ASSISTENTE.Infrastructure.PromptGenerator.Prompts;

internal sealed class ContextPreProcessingPrompt : IPrompt
{
    public PromptType Type => PromptType.ContextPreProcessing;

    public string Generate(string question, string context)
    {
        const string wholeDocument = "";
        const string chunk = "";
        
        var prompt = $"""
                      <document>
                      {wholeDocument}
                      </document>

                      Here is the chunk we want to situate within the whole document
                      
                      <chunk>
                      {chunk}
                      </chunk>
                      
                      Please give a short succinct context to situate this chunk within the overall document for the
                      purpose of improving search retrieval of the chunk. Answer only with the succinct context 
                      and nothing else. 
                      """;

        // TODO: Instead of providing chunk without context, provide chunk with context for better understanding by LLM
        
        return prompt;
    }
}