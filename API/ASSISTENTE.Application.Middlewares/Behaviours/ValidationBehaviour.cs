using CSharpFunctionalExtensions;
using FluentValidation;
using MediatR;
using SOFTURE.Results;

namespace ASSISTENTE.Application.Middlewares.Behaviours
{
    internal class CommandValidationBehaviour<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
        : IPipelineBehavior<TRequest, Result>
        where TRequest : IRequest<Result>
    {
        public async Task<Result> Handle(
            TRequest request, 
            RequestHandlerDelegate<Result> next, 
            CancellationToken cancellationToken)
        {
            if (!validators.Any()) return await next();
            
            var context = new ValidationContext<TRequest>(request);

            var failures = validators
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(f => f != null)
                .Select(f => new Error(f.PropertyName, f.ErrorMessage).Build())
                .ToList();

            if (failures.Count != 0)
            {
                return Result.Failure<TResponse>(failures.First());
            }

            return await next();
        }
    }
}
