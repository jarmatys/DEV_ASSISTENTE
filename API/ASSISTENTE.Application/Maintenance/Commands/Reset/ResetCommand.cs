using ASSISTENTE.Infrastructure.Interfaces;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ASSISTENTE.Application.Maintenance.Commands.Reset
{
    public sealed class ResetCommand : IRequest<Result>
    {
        private ResetCommand()
        {
        }
        
        public static ResetCommand Create()
        {
            return new ResetCommand();
        }
    }

    public class ResetCommandHandler(IMaintenanceService maintenanceService, ILogger<ResetCommandHandler> logger)
        : IRequestHandler<ResetCommand, Result>
    {
        public async Task<Result> Handle(ResetCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Resetting the playground...");

            var resetResult = await maintenanceService.ResetAsync();
            var initResult = await maintenanceService.InitAsync();

            logger.LogInformation("Playground reset!");
            
            return Result.Combine(resetResult, initResult);
        }
    }
}