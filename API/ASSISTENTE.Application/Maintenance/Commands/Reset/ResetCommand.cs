using ASSISTENTE.Infrastructure.Interfaces;
using CSharpFunctionalExtensions;
using MediatR;

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

    public class ResetCommandHandler(IMaintenanceService maintenanceService)
        : IRequestHandler<ResetCommand, Result>
    {
        public async Task<Result> Handle(ResetCommand request, CancellationToken cancellationToken)
        {
            // TODO: Add serilog and use logger instead of Console.WriteLine

            Console.WriteLine("\nResetting the playground...");

            var resetResult = await maintenanceService.ResetAsync();
            var initResult = await maintenanceService.InitAsync();

            Console.WriteLine("\nPlayground reset!");
            
            return Result.Combine(resetResult, initResult);
        }
    }
}