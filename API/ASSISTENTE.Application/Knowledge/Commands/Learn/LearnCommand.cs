using ASSISTENTE.Domain.Entities.Resources.Enums;
using ASSISTENTE.Infrastructure.Interfaces;
using CSharpFunctionalExtensions;
using MediatR;

namespace ASSISTENTE.Application.Knowledge.Commands.Learn
{
    public sealed class LearnCommand : IRequest<Result>
    {
        private LearnCommand()
        {
        }
        
        public static LearnCommand Create()
        {
            return new LearnCommand();
        }
    }

    public class LearnCommandHandler(
        IFileParser fileParser, 
        IKnowledgeService knowledgeService)
        : IRequestHandler<LearnCommand, Result>
    {
        public async Task<Result> Handle(LearnCommand request, CancellationToken cancellationToken)
        {
            // TODO: Add serilog and use logger instead of Console.WriteLine
            
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
            
            return Result.Combine(notesLearnResult, codeLearnResult);
        }
    }
}