using ASSISTENTE.Common.Results;
using ASSISTENTE.Language.Common;
using Stateless;

namespace ASSISTENTE.Domain.Common;

public abstract class StatefulEntity<TIdentifier, TState, TAction> : AuditableEntity<TIdentifier>
    where TIdentifier : class, IIdentifier
    where TState : Enum
    where TAction : Enum
{
    protected StatefulEntity()
    {
        StateMachine = new StateMachine<TState, TAction>(() => State, s => State = s);
    }

    protected readonly StateMachine<TState, TAction> StateMachine;

    public TState State { get; protected set; } = default!;

    protected virtual void UpdateDomainTimestamp()
    {
    }

    private static Result HasPermission(TAction action) => Result.Success();

    private Result CheckIfPossible(TAction action, Error cannotFireError)
    {
        return HasPermission(action)
            .Bind(() => StateMachine.CanFire(action)
                ? Result.Success()
                : Result.Failure(cannotFireError.Build($"State: {State}"))
            );
    }

    private Result Perform(TAction action)
    {
        StateMachine.Fire(action);

        UpdateDomainTimestamp();

        return Result.Success();
    }

    protected Result PerformIfPossible(TAction action, Error cannotFireError)
    {
        return CheckIfPossible(action, cannotFireError)
            .Bind(() => Perform(action));
    }
}