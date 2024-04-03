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

    public class LearnCommandHandler : IRequestHandler<LearnCommand, Result>
    {
        public async Task<Result> Handle(LearnCommand request, CancellationToken cancellationToken)
        {
            return Result.Success();
        }
    }
}