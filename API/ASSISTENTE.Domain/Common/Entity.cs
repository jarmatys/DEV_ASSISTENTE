using ASSISTENTE.Domain.Common.Interfaces;
using ASSISTENTE.Language;

namespace ASSISTENTE.Domain.Common;

public abstract class Entity<TIdentifier> : IEntity
    where TIdentifier : class, IIdentifier
{
    public TIdentifier Id { get; set; } = null!;

    private readonly List<IDomainEvents> _domainEvents = [];

    public IReadOnlyList<IDomainEvents> GetEvents()
    {
        return _domainEvents.ToList();
    }

    public void ClearEvents()
    {
        _domainEvents.Clear();
    }

    internal void RaiseEvent(IDomainEvents domainEvents)
    {
        _domainEvents.Add(domainEvents);
    }
}