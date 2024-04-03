using ASSISTENTE.Domain.Entities.Resources.Enums;
using ASSISTENTE.Infrastructure.Interfaces;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Client;

public sealed class Playground(
    IFileParser fileParser,
    IKnowledgeService knowledgeService,
    IMaintenanceService maintenanceService)
{
    public async Task AnswerAsync(string question)
    {
        Console.WriteLine($"\nQuestion: '{question}'");
        
        // TODO: Use here MediatR to dispatch a query to get the answer
        
        await knowledgeService.RecallAsync(question)
            .Tap(answer => Console.WriteLine($"\nAnswer: '{answer}'"))
            .TapError(errors => Console.WriteLine(errors));
    }

    public async Task LearnAsync()
    {
        // TODO: Use here MediatR to dispatch a command to learn from the file

        var notesLearnResult = await fileParser
            .GetNotes()
            .Bind(async resources =>
            {
                var results = new List<Result>();
                
                foreach (var resource in resources)
                {
                    var learnResult = await knowledgeService.LearnAsync(resource, ResourceType.Note);
                    
                    results.Add(learnResult);

                    Console.WriteLine($"\nLearned from note: '{resource.Title}'");
                }
        
                return Result.Combine(results);
            });
        
        var codeLearnResult = await fileParser
            .GetCode()
            .Bind(async resources =>
            {
                var results = new List<Result>();
                
                foreach (var resource in resources)
                {
                    var learnResult = await knowledgeService.LearnAsync(resource, ResourceType.Code);
                    
                    results.Add(learnResult);

                    Console.WriteLine($"\nLearned from code: '{resource.Title}'");
                }
        
                return Result.Combine(results);
            });
    }

    public async Task ResetAsync()
    {
        Console.WriteLine("\nResetting the playground...");
        
        await maintenanceService.ResetAsync();
        await maintenanceService.InitAsync();
        
        Console.WriteLine("\nPlayground reset!");
    }
}