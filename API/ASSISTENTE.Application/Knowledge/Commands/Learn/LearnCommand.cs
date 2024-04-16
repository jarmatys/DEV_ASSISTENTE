using ASSISTENTE.Application.Abstractions.Interfaces;
using ASSISTENTE.Domain.Entities.Resources.Enums;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.Logging;

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
        IKnowledgeService knowledgeService,
        ILogger<LearnCommandHandler> logger)
        : IRequestHandler<LearnCommand, Result>
    {
        public async Task<Result> Handle(LearnCommand request, CancellationToken cancellationToken)
        {
            var notesLearnResult = await fileParser
                .GetNotes()
                .Bind(async resources =>
                {
                    var results = new List<Result>();
                
                    foreach (var resource in resources)
                    {
                        var learnResult = await knowledgeService.LearnAsync(resource, ResourceType.Note);
                    
                        results.Add(learnResult);

                        logger.LogInformation("Learned from note: '{ResourceTitle}'", resource.Title);
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

                        logger.LogInformation("Learned from code: '{ResourceTitle}'", resource.Title);
                    }
        
                    return Result.Combine(results);
                });
            
            return Result.Combine(notesLearnResult, codeLearnResult);
        }
    }
}